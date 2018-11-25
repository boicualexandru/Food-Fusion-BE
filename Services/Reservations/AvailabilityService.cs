﻿using AutoMapper;
using Common;
using Common.ConcurrentEvents;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Reservations.Models;
using Services.Restaurants.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Reservations
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConcurrentEventsService<ReservationDetailedModel> _concurrentEventsService;

        public AvailabilityService(
            FoodFusionContext dbContext, 
            IMapper mapper, 
            IConcurrentEventsService<ReservationDetailedModel> concurrentEventsService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _concurrentEventsService = concurrentEventsService;
        }

        public IList<TimeRange> GetUnavailableTimeRanges(int restaurantId, int participantsCount, TimeRange timeRange)
        {
            var restaurant = _dbContext.Restaurants
                .AsNoTracking()
                .Include(r => r.Map)
                    .ThenInclude(m => m.Tables)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            var tables = _mapper.Map<List<TableModel>>(restaurant.Map.Tables);
            if (!DoesParticipantsFitToTables(tables, participantsCount))
            {
                return new List<TimeRange> { timeRange };
            }

            // TODO: improve this to be done in one query
            bool isReservationInRange(Reservation res) => res.EndTime > timeRange.Start ||
                    res.StartTime > timeRange.End;

            var reservations = _dbContext.Reservations
                .AsNoTracking()
                .Include(r => r.ReservedTables)
                    .ThenInclude(rt => rt.Table)
                .Where(r => r.RestaurantId == restaurantId)
                .Where(isReservationInRange)
                .ToList();

            var detailedReservations = _mapper.Map<IList<ReservationDetailedModel>>(reservations);

            var tablesAvailabilityList = GetTablesAvailabilityList(tables, detailedReservations);

            var unavailableTimeRanges = new List<TimeRange>();
            foreach (var tablesAvailability in tablesAvailabilityList)
            {
                if (!DoesParticipantsFitToTables(tablesAvailability.FreeTables, participantsCount))
                {
                    unavailableTimeRanges.Add(tablesAvailability.Range);
                }
            }

            return unavailableTimeRanges;
        }

        private IList<TablesAvailability> GetTablesAvailabilityList(
            IList<TableModel> tables, 
            IList<ReservationDetailedModel> existingReservations)
        {
            var concurrentReservationsList = _concurrentEventsService.GetConcurrentEvents(existingReservations);

            var tablesAvailabilityList = new List<TablesAvailability>();

            foreach (var concurrentReservations in concurrentReservationsList)
            {
                var reservedTablesIds = concurrentReservations.Events
                    .SelectMany(r => r.Tables)
                    .Select(t => t.Id);

                var freeTables = tables.Where(t => !reservedTablesIds.Contains(t.Id));

                var tablesAvailability = new TablesAvailability
                {
                    Range = concurrentReservations.Range,
                    FreeTables = freeTables.ToList()
                };
                tablesAvailabilityList.Add(tablesAvailability);
            }

            return tablesAvailabilityList;
        }

        private bool DoesParticipantsFitToTables(List<TableModel> tables, int participantsCount, bool canSplit = true)
        {
            if (canSplit)
            {
                var availableSeatsCount = tables.Select(t => t.Seats).Sum();
                return availableSeatsCount >= participantsCount;
            }

            var isAnyTableFit = tables.Any(t => t.Seats >= participantsCount);
            return isAnyTableFit;
        }
    }
}