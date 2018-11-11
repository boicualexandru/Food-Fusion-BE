using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Services.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Restaurants.Exceptions;
using System.Security.Claims;
using Services.Authorization.Exceptions;

namespace Services.Restaurants
{
    /// <summary>
    /// Authorizations:
    /// Read            - All Users
    /// Create, Delete  - Admin
    /// Update          - Manager
    /// </summary>
    public class RestaurantAuthorizationHandler :
    AuthorizationHandler<RestaurantAuthorizationRequirement, int>
    {
        private readonly FoodFusionContext _dbContext;

        public RestaurantAuthorizationHandler(FoodFusionContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RestaurantAuthorizationRequirement requirement,
                                                       int restaurantId = 0)
        {
            // Grant read for all
            if(requirement.Name == Operations<RestaurantAuthorizationRequirement>.Read.Name)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Grant all rights for Admin
            var isAdmin = context.User.IsInRole(UserRole.Admin.ToString());
            if (isAdmin)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if(requirement.Name != Operations<RestaurantAuthorizationRequirement>.Update.Name)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) throw new InvalidClaimException();

            var restaurant = _dbContext.Restaurants
                .AsNoTracking()
                .Include(r => r.Manager)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();
            restaurant.Manager = restaurant.Manager ?? throw new ManagerNotFoundException();
            
            if (userId != restaurant.Manager.Id)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // Grant Update for Restaurant Manager
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class RestaurantAuthorizationRequirement : OperationAuthorizationRequirement { }
    //public abstract class RestaurantOperations : Operations<RestaurantAuthorizationRequirement> { }
}
