using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebApi.ActionFilters
{
    public class AuthorizeByMenuAttribute : TypeFilterAttribute
    {
        public AuthorizeByMenuAttribute(string roles, string key = "menuId") : base(typeof(AuthorizeByMenuFilter))
        {
            Arguments = new object[] { roles, key };
        }

        private class AuthorizeByMenuFilter : AuthorizeByRestaurantAttribute
        {
            private readonly FoodFusionContext _dbContext;

            public AuthorizeByMenuFilter(FoodFusionContext dbContext, string roles, string key) : base(roles, key)
            {
                _dbContext = dbContext;
            }
            
            override protected string GetRestaurantId(string keyValue)
            {
                var restaurantId = _dbContext.Menus
                    .AsNoTracking()
                    .Where(m => m.Id == int.Parse(keyValue))
                    .Select(m => m.RestaurantId)
                    .FirstOrDefault();

                return restaurantId.ToString();
            }
        }
    }
}
