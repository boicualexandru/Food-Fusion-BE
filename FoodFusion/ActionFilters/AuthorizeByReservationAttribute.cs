using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace WebApi.ActionFilters
{
    public class AuthorizeByReservationAttribute : TypeFilterAttribute
    {
        public AuthorizeByReservationAttribute(string roles = "", string reqRoles = "", string key = "reservationId") : base(typeof(AuthorizeByReservationFilter))
        {
            Arguments = new object[] { roles, reqRoles, key };
        }

        private class AuthorizeByReservationFilter : AuthorizeByRestaurantAttribute
        {
            private readonly FoodFusionContext _dbContext;

            public AuthorizeByReservationFilter(FoodFusionContext dbContext, string roles, string reqRoles, string key) : base(roles, reqRoles, key)
            {
                _dbContext = dbContext;
            }

            override protected string GetRestaurantId(string keyValue)
            {
                var restaurantId = _dbContext.Reservations
                    .AsNoTracking()
                    .Where(r => r.RestaurantId == int.Parse(keyValue))
                    .Select(r => r.RestaurantId)
                    .FirstOrDefault();

                return restaurantId.ToString();
            }

            override protected bool AuthorizeAdditionalRoles()
            {
                var requireOwner = _roles.Contains("Owner");
                if (!requireOwner) return false;

                var userIdClaim = _user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userId = int.Parse(userIdClaim);

                var isOwnerOfTheReservation = _dbContext.Reservations
                    .AsNoTracking()
                    .Any(r => r.Id == int.Parse(_restaurantId) && r.UserId == userId);
                if (isOwnerOfTheReservation) return true;

                return false;
            }
        }
    }
}
