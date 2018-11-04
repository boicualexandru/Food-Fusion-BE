using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (restaurantId == 3)
            {
                context.Succeed(requirement);
            }

            //if (context.User.Identity?.Name == resource.Author)
            //{
            //    context.Succeed(requirement);
            //}

            return Task.CompletedTask;
        }
    }

    public class RestaurantAuthorizationRequirement : OperationAuthorizationRequirement { }
}
