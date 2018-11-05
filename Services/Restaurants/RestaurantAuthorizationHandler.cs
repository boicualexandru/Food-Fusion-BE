using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Authorization;

namespace Services.Restaurants
{
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
            if(requirement.Name == Operations<RestaurantAuthorizationRequirement>.Read.Name)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var isAdmin = context.User.IsInRole(UserRole.Admin.ToString());
            if (isAdmin)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //if (context.User.Identity?.Name == resource.Author)
            //{
            //    context.Succeed(requirement);
            //}

            return Task.CompletedTask;
        }
    }

    public class RestaurantAuthorizationRequirement : OperationAuthorizationRequirement { }
    //public abstract class RestaurantOperations : Operations<RestaurantAuthorizationRequirement> { }
}
