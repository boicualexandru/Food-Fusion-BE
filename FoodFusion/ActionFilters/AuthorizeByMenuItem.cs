using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebApi.ActionFilters
{
    public class AuthorizeByMenuItemAttribute : TypeFilterAttribute
    {
        public AuthorizeByMenuItemAttribute(string roles = null, string reqRoles = null, string key = "itemId") : base(typeof(AuthorizeByMenuItemFilter))
        {
            Arguments = new object[] { roles, reqRoles, key };
        }

        private class AuthorizeByMenuItemFilter : AuthorizeByRestaurantAttribute
        {
            private readonly FoodFusionContext _dbContext;

            public AuthorizeByMenuItemFilter(FoodFusionContext dbContext, string roles, string reqRoles, string key) : base(roles, reqRoles, key)
            {
                _dbContext = dbContext;
            }

            override protected string GetRestaurantId(string keyValue)
            {
                var restaurantId = _dbContext.MenuItems
                    .AsNoTracking()
                    .Include(mi => mi.Menu)
                    .Where(mi => mi.Id == int.Parse(keyValue))
                    .Select(m => m.Menu.RestaurantId)
                    .FirstOrDefault();

                return restaurantId.ToString();
            }
        }
    }
}
