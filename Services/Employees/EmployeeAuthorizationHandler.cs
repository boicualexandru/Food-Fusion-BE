using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Services.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Services.Authorization.Exceptions;
using Services.Restaurants.Exceptions;

namespace Services.Employees
{
    //TODO: Add it as a sub operation in Restaurant Authorization
    /// <summary>
    /// Authorizations:
    /// Read                    - Employees, Manager
    /// Create, Update, Delete  - Manager
    /// </summary>
    public class EmployeeAuthorizationHandler :
    AuthorizationHandler<EmployeeAuthorizationRequirement, int>
    {
        private readonly FoodFusionContext _dbContext;

        public EmployeeAuthorizationHandler(FoodFusionContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       EmployeeAuthorizationRequirement requirement,
                                                       int restaurantId = 0)
        {
            // Grant all rights for Admin
            var isAdmin = context.User.IsInRole(UserRole.Admin.ToString());
            if (isAdmin)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) throw new InvalidClaimException();

            var restaurant = _dbContext.Restaurants
                .Include(r => r.RestaurantEmployees)
                .FirstOrDefault(i => i.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            // Grant all rights for Manager
            if (restaurant.ManagerId == userId)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Grant read for Employees
            if(requirement.Name == Operations<EmployeeAuthorizationRequirement>.Read.Name
                && restaurant.RestaurantEmployees.Any(re => re.UserId == userId))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }

    public class EmployeeAuthorizationRequirement : OperationAuthorizationRequirement { }
}
