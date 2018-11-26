using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Common.ConcurrentEvents;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Authentication.Exceptions;
using Services.Reservations.Exceptions;
using Services.Reservations.Models;
using Services.Restaurants.Exceptions;
using Services.Tables.Exceptions;

namespace Services.Reservations
{
    public class ReservationsService : IReservationsService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAvailabilityService _availabilityService;

        public ReservationsService(
            FoodFusionContext dbContext, 
            IMapper mapper, 
            IConcurrentEventsService<ReservationDetailedModel> concurrentEventsService,
            IAvailabilityService availabilityService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _availabilityService = availabilityService;
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

        public ReservationDetailedModel AddReservation(ReservationRequestModel reservationModel)
        {
            if (reservationModel.TableIds == null || reservationModel.TableIds.Count == 0)
            {
                // no tables were selected
                throw new TableNotFoundException();
            }

            reservationModel.TableIds = reservationModel.TableIds.Distinct().ToList();

            var tablesFromDb = _dbContext.RestaurantTables
                .Include(t => t.Map)
                .Where(t => reservationModel.TableIds.Contains(t.Id))
                .ToList();

            // check that all the tables are present in the DB
            if(reservationModel.TableIds.Count != tablesFromDb.Count)
            {
                throw new TableNotFoundException();
            }

            // check if the table assignation is optimal
            var areTooManyTablesRequested =
                _availabilityService.AreTooManyTablesRequested(tablesFromDb, reservationModel.ParticipantsCount ?? 0);
            if (areTooManyTablesRequested)
            {
                throw new TooManyTablesRequestedException();
            }

            // check that the tables are belonging to this restaurant
            var doTablesBelongToThisRestaurant = tablesFromDb
                .All(t => t.Map.RestaurantId == reservationModel.RestaurantId);

            if (!doTablesBelongToThisRestaurant)
            {
                throw new TableNotFoundException();
            }
            
            var areTablesAvailable = _availabilityService.AreTablesAvailable(reservationModel.TableIds, reservationModel.Range);
            if (!areTablesAvailable) throw new ReservationNotAvalableException();

            var reservation = _mapper.Map<Reservation>(reservationModel);

            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();

            _dbContext.Entry(reservation).Reference(p => p.Restaurant).Load();
            _dbContext.Entry(reservation).Reference(p => p.User).Load();

            return _mapper.Map<ReservationDetailedModel>(reservation);
        }

        public ReservationDetailedModel GetReservation(int reservationId)
        {
            var reservation = _dbContext.Reservations
                .AsNoTracking()
                .Include(r => r.Restaurant)
                .Include(r => r.User)
                .Include(r => r.ReservedTables)
                    .ThenInclude(rt => rt.Table)
                .FirstOrDefault(r => r.Id == reservationId);
            reservation = reservation ?? throw new ReservationNotFoundException();

            return _mapper.Map<ReservationDetailedModel>(reservation);
        }

        public void RemoveReservation(int reservationId)
        {
            var reservation = _dbContext.Reservations
                .FirstOrDefault(r => r.Id == reservationId);
            reservation = reservation ?? throw new ReservationNotFoundException();

            _dbContext.Reservations.Remove(reservation);
            _dbContext.SaveChanges();
        }

        public ReservationDetailedModel UpdateReservation(ReservationRequestModel reservation)
        {
            //var restaurant = _dbContext.Restaurants
            //    .FirstOrDefault(r => r.Id == restaurantModel.Id);
            //restaurant = restaurant ?? throw new RestaurantNotFoundException();

            //restaurant = _mapper.Map(restaurantModel, restaurant);

            //_dbContext.SaveChanges();

            throw new NotImplementedException();
        }
    }
}
