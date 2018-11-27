using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Services.Authorization;
using Services.Authorization.Exceptions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Reservations
{

    /// <summary>
    /// Authorizations:
    /// Create                  - All Users
    /// Read, Update, Delete    - Employees, Manager, Owner of Reservation
    /// </summary>
    public class ReservationAuthorizationHandler :
    AuthorizationHandler<ReservationAuthorizationRequirement, int>
    {
        private readonly FoodFusionContext _dbContext;

        public ReservationAuthorizationHandler(FoodFusionContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       ReservationAuthorizationRequirement requirement,
                                                       int reservationId = 0)
        {
            // Grant all rights for Admin
            var isAdmin = context.User.IsInRole(UserRole.Admin.ToString());
            if (isAdmin)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Grant Create for All Users
            if (requirement.Name == Operations<ReservationAuthorizationRequirement>.Create.Name)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) throw new InvalidClaimException();

            bool isOwnerOfReservation() =>
                _dbContext.Reservations
                    .Where(r => r.Id == reservationId && r.UserId == userId)
                    .Any();

            bool isManagerOfRestaurant() =>
                _dbContext.Reservations
                    .Include(r => r.Restaurant)
                    .Where(r => r.Id == reservationId && r.Restaurant.ManagerId == userId)
                    .Any();

            bool isEmployeeOfRestaurant() =>
                _dbContext.Reservations
                    .Include(r => r.Restaurant)
                        .ThenInclude(r => r.RestaurantEmployees)
                    .Where(r => r.Id == reservationId && r.Restaurant.RestaurantEmployees.Select(re => re.UserId).Contains(userId))
                    .Any();

            // Grant All Rights for Owner of Reservation, Restaurant Manager and Restaurant Employee
            if (isOwnerOfReservation() ||
               isManagerOfRestaurant() ||
               isEmployeeOfRestaurant())
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }

    public class ReservationAuthorizationRequirement : OperationAuthorizationRequirement { }
}
