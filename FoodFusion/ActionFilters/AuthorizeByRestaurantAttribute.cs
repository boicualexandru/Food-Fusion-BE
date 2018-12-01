using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Authentication.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace WebApi.ActionFilters
{
    public class AuthorizeByRestaurantAttribute : Attribute, IResourceFilter
    {
        public string Roles
        {
            get { return string.Join(',', _roles); }
            set { _roles = value.Split(',').Select(r => r.Trim()).ToArray(); }
        }
        public string Key { get; set; }

        private string[] _roles;

        public AuthorizeByRestaurantAttribute() { }

        public AuthorizeByRestaurantAttribute(string roles, string key = "restaurantId")
        {
            Roles = roles;
            Key = Key;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {

            var requireAdmin = _roles.Contains("Admin");
            if (requireAdmin)
            {
                var hasAdminRole = context.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
                if (hasAdminRole) return;
            }
            
            var keyValue = context.RouteData.Values[Key].ToString();
            var restaurantId = GetRestaurantId(keyValue);

            var requireManager = _roles.Contains("Manager");
            if (requireManager)
            {
                var isManagerForThisRestaurant = context.HttpContext
                        .User.Claims.Any(c => c.Type == CustomDefinedClaimNames.ManagedRestaurant && c.Value == restaurantId);
                if (isManagerForThisRestaurant) return;
            }

            var requireEmployee = _roles.Contains("Employee");
            if (requireEmployee)
            {
                var isEmployeeForThisRestaurant = context.HttpContext
                        .User.Claims.Any(c => c.Type == CustomDefinedClaimNames.EmployeeOfRestaurant && c.Value == restaurantId);
                if (isEmployeeForThisRestaurant) return;
            }
            
            context.Result = new ForbidResult();
        }

        public void OnResourceExecuted(ResourceExecutedContext context) { }

        protected virtual string GetRestaurantId(string keyValue) => keyValue;
    }
}
