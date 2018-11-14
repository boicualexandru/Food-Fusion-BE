using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Services.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Services.Menus.Exceptions;
using Services.Authorization.Exceptions;
using Services.Employees.Exceptions;

namespace Services.Menus
{
    /// <summary>
    /// Authorizations:
    /// Read                    - All Users
    /// Create, Update, Delete  - Manager
    /// </summary>
    public class MenuItemAuthorizationHandler :
    AuthorizationHandler<MenuItemAuthorizationRequirement, int>
    {
        private readonly FoodFusionContext _dbContext;

        public MenuItemAuthorizationHandler(FoodFusionContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       MenuItemAuthorizationRequirement requirement,
                                                       int menuItemId = 0)
        {
            // Grant read for all
            if(requirement.Name == Operations<MenuItemAuthorizationRequirement>.Read.Name)
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

            var menuItem = _dbContext.MenuItems
                .AsNoTracking()
                .Include(i => i.Menu)
                    .ThenInclude(m => m.Restaurant)
                .FirstOrDefault(i => i.Id == menuItemId);
            menuItem = menuItem ?? throw new MenuItemNotFoundException();
            menuItem.Menu.Restaurant.ManagerId = menuItem.Menu.Restaurant.ManagerId ?? throw new ManagerNotFoundException();
            
            if (userId != menuItem.Menu.Restaurant.ManagerId)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // Grant All Rights for Restaurant Manager
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class MenuItemAuthorizationRequirement : OperationAuthorizationRequirement { }
}
