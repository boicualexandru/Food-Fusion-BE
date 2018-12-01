using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Authentication.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace WebApi.ActionFilters
{
    public class AuthorizeByRestaurantAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        protected readonly string[] _roles;
        protected readonly string _key;

        protected ClaimsPrincipal _user;
        protected string _restaurantId;


        public AuthorizeByRestaurantAttribute() { }

        public AuthorizeByRestaurantAttribute(string roles, string key = "restaurantId")
        {
            _roles = roles.Split(',').Select(r => r.Trim()).ToArray();
            _key = key;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _user = context.HttpContext.User;

            if (!_user.Identity.IsAuthenticated) return;

            var requireAdmin = _roles.Contains("Admin");
            if (requireAdmin)
            {
                var hasAdminRole = _user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
                if (hasAdminRole) return;
            }
            
            var keyValue = context.RouteData.Values[_key].ToString();
            _restaurantId = GetRestaurantId(keyValue);

            var requireManager = _roles.Contains("Manager");
            if (requireManager)
            {
                var isManagerForThisRestaurant = context.HttpContext
                        .User.Claims.Any(c => c.Type == CustomDefinedClaimNames.ManagedRestaurant && c.Value == _restaurantId);
                if (isManagerForThisRestaurant) return;
            }

            var requireEmployee = _roles.Contains("Employee");
            if (requireEmployee)
            {
                var isEmployeeForThisRestaurant = context.HttpContext
                        .User.Claims.Any(c => c.Type == CustomDefinedClaimNames.EmployeeOfRestaurant && c.Value == _restaurantId);
                if (isEmployeeForThisRestaurant) return;
            }

            if (AuthorizeAdditionalRoles()) return;
            
            context.Result = new ForbidResult();
        }

        protected virtual string GetRestaurantId(string keyValue) => keyValue;

        protected virtual bool AuthorizeAdditionalRoles() => false;
    }
}
