﻿using System;
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

        public ReservationDetailedModel AddReservation(ReservationRequestModel reservationRequest)
        {
            ValidateReservationRequest(reservationRequest);

            var areTablesAvailable = _availabilityService.AreTablesAvailable(reservationRequest.TableIds, reservationRequest.Range);
            if (!areTablesAvailable) throw new ReservationNotAvalableException();

            var reservation = _mapper.Map<Reservation>(reservationRequest);

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

        public ReservationDetailedModel UpdateReservation(ReservationRequestModel reservationRequest)
        {
            var reservation = _dbContext.Reservations
                .Include(r => r.ReservedTables)
                .Include(r => r.Restaurant)
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == reservationRequest.Id);
            reservation = reservation ?? throw new ReservationNotFoundException();

            // override properties that are not changeable
            reservationRequest.UserId = reservation.UserId;
            reservationRequest.RestaurantId = reservation.RestaurantId;

            ValidateReservationRequest(reservationRequest);

            // check if the newly added tables are available
            var oldTableIds = reservation.ReservedTables
                .Select(rt => rt.RestaurantTableId).ToList();
            var updatedTableIds = reservationRequest.TableIds;

            var oldTableReservations = reservation.ReservedTables.Select(rt => rt.Id).ToList();
            var areTablesAvailable = _availabilityService.AreTablesAvailable(updatedTableIds, reservationRequest.Range, oldTableReservations);
            if (!areTablesAvailable) throw new ReservationNotAvalableException();

            reservation = _mapper.Map(reservationRequest, reservation);

            _dbContext.SaveChanges();

            return _mapper.Map<ReservationDetailedModel>(reservation);
        }

        private void ValidateReservationRequest(ReservationRequestModel reservationRequest)
        {
            if (reservationRequest.TableIds == null || reservationRequest.TableIds.Count == 0)
            {
                // no tables were selected
                throw new TableNotFoundException();
            }

            reservationRequest.TableIds = reservationRequest.TableIds.Distinct().ToList();

            var tablesFromDb = _dbContext.RestaurantTables
                .Include(t => t.Map)
                .Where(t => reservationRequest.TableIds.Contains(t.Id))
                .ToList();

            // check that all the tables are present in the DB
            if (reservationRequest.TableIds.Count != tablesFromDb.Count)
            {
                throw new TableNotFoundException();
            }

            // check if the number of participants could fit the table's seats
            var availableSeatsCount = tablesFromDb.Select(t => t.Seats).Sum();
            var areTablesFittingTheNumberOfParticipants = availableSeatsCount >= reservationRequest.ParticipantsCount;
            if (!areTablesFittingTheNumberOfParticipants)
            {
                throw new TablesAreNotFittingException();
            }

            // check if the table assignation is optimal (if there could be less tables requested)
            var areTooManyTablesRequested =
                _availabilityService.AreTooManyTablesRequested(tablesFromDb, reservationRequest.ParticipantsCount ?? 0);
            if (areTooManyTablesRequested)
            {
                throw new TooManyTablesRequestedException();
            }

            // check that the tables are belonging to this restaurant
            var doTablesBelongToThisRestaurant = tablesFromDb
                .All(t => t.Map.RestaurantId == reservationRequest.RestaurantId);

            if (!doTablesBelongToThisRestaurant)
            {
                throw new TableNotFoundException();
            }
        }
    }
}
