using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Common.ConcurrentEvents;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Authentication.Exceptions;
using Services.Reservations.Models;
using Services.Restaurants.Exceptions;

namespace Services.Reservations
{
    public class ReservationsService : IReservationsService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IMapper _mapper;

        public ReservationsService(
            FoodFusionContext dbContext, 
            IMapper mapper, 
            IConcurrentEventsService<ReservationDetailedModel> concurrentEventsService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<ReservationModel> GetRestaurantReservations(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .AsNoTracking()
                .Include(r => r.Reservations)
                    .ThenInclude(r => r.User)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            return _mapper.Map<IList<ReservationModel>>(restaurant.Reservations);
        }

        public IList<ReservationModel> GetUserReservations(int userId)
        {
            var restaurant = _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Reservations)
                    .ThenInclude(r => r.User)
                .FirstOrDefault(u => u.Id == userId);
            restaurant = restaurant ?? throw new UserNotFoundException();

            return _mapper.Map<IList<ReservationModel>>(restaurant.Reservations);
        }

        public ReservationDetailedModel AddReservation(ReservationDetailedModel reservation)
        {
            throw new NotImplementedException();
        }

        public ReservationDetailedModel GetReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

        public void RemoveReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

        public ReservationDetailedModel UpdateReservation(ReservationDetailedModel reservation)
        {
            throw new NotImplementedException();
        }
    }
}
