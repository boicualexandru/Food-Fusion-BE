using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Services.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Restaurants.Exceptions;
using System.Security.Claims;
using Services.Menus.Exceptions;
using Services.Authorization.Exceptions;

namespace Services.Menus
{
    /// <summary>
    /// Authorizations:
    /// Read                    - All Users
    /// Create, Update, Delete  - Manager
    /// </summary>
    public class MenuAuthorizationHandler :
    AuthorizationHandler<MenuAuthorizationRequirement, int>
    {
        private readonly FoodFusionContext _dbContext;

        public MenuAuthorizationHandler(FoodFusionContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       MenuAuthorizationRequirement requirement,
                                                       int menuId = 0)
        {
            // Grant read for all
            if(requirement.Name == Operations<MenuAuthorizationRequirement>.Read.Name)
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

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) throw new InvalidClaimException();

            var menu = _dbContext.Menus
                .AsNoTracking()
                .Include(m => m.Restaurant)
                    .ThenInclude(r => r.Manager)
                .FirstOrDefault(m => m.Id == menuId);
            menu = menu ?? throw new MenuNotFoundException();
            menu.Restaurant = menu.Restaurant ?? throw new RestaurantNotFoundException();
            menu.Restaurant.Manager = menu.Restaurant.Manager ?? throw new ManagerNotFoundException();
            
            if (userId != menu.Restaurant.Manager.Id)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // Grant All Rights for Restaurant Manager
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class MenuAuthorizationRequirement : OperationAuthorizationRequirement { }
}
