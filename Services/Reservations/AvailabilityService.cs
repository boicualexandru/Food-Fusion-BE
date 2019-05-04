using AutoMapper;
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
            bool isReservationInRange(Reservation res) => res.StartTime < timeRange.End &&
                    res.EndTime > timeRange.Start;

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

            var midnight = timeRange.Start.Date;
            while(midnight < timeRange.End)
            {
                unavailableTimeRanges.Add(new TimeRange
                {
                    Start = midnight,
                    End = midnight + TimeSpan.FromHours(7)
                });
                midnight += TimeSpan.FromDays(1);
            }

            return unavailableTimeRanges;
        }

        /// <summary>
        /// Checks if all the given tables are available for the given time range
        /// </summary>
        /// <param name="tableIds">Ids of tables that will be checked if they are valid for specified time range</param>
        /// <param name="range">Time range on which the availability will be checked</param>
        /// <param name="reservedTablesToExclude">Ids of reserved tables that will be excluded from checking if they overlap with the requested tables</param>
        /// <returns></returns>
        public bool AreTablesAvailable(IList<int> tableIds, TimeRange range, IList<int> reservedTableidsToExclude = null)
        {
            var reservedTables = _dbContext.ReservedTables
                .AsNoTracking()
                .Include(rt => rt.Reservation)
                .Where(rt => tableIds.Contains(rt.RestaurantTableId));

            if(reservedTableidsToExclude?.Count > 0)
            {
                reservedTables = reservedTables.Where(rt => !reservedTableidsToExclude.Contains(rt.Id));
            }

            var isAnyReservationOverlapping = reservedTables
                .Any(rt => rt.Reservation.StartTime < range.End && rt.Reservation.EndTime > range.Start);

            return !isAnyReservationOverlapping;
        }

        public bool AreTooManyTablesRequested(IList<RestaurantTable> tables, int participantsCount)
        {
            var tablesSeatsCount = tables.Select(t => t.Seats).OrderByDescending(s => s);

            var seatsLeftToFit = participantsCount;
            foreach (var seatsCount in tablesSeatsCount)
            {
                if (seatsLeftToFit <= 0) return true;

                seatsLeftToFit -= seatsCount;
            }

            return false;
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

        public IList<TableModel> GetAvailableTables(int restaunrantId, TimeRange range, int participantsCount)
        {
            var restaurantTables = _dbContext.RestaurantTables
                .AsNoTracking()
                .Include(t => t.Map)
                .Include(t => t.ReservedTables)
                    .ThenInclude(rt => rt.Reservation)
                .Where(rt => rt.Map.RestaurantId == restaunrantId);

            var fittingTables = restaurantTables.Where(t => t.Seats >= participantsCount);

            var availableTables = fittingTables
                .Where(t => !t.ReservedTables
                    .Any(rt => rt.Reservation.StartTime < range.End && rt.Reservation.EndTime > range.Start));

            return _mapper.Map<List<TableModel>>(availableTables.ToList());
        }

        public IList<TableStatus> GetTablesStatus(int restaunrantId, TimeRange range, int participantsCount)
        {
            var restaurantTables = _dbContext.RestaurantTables
                   .AsNoTracking()
                   .Include(t => t.Map)
                   .Include(t => t.ReservedTables)
                       .ThenInclude(rt => rt.Reservation)
                   .Where(rt => rt.Map.RestaurantId == restaunrantId);

            var fittingTables = restaurantTables.Where(t => t.Seats >= participantsCount);
            
            var availableTables = fittingTables
                .Where(t => !t.ReservedTables
                    .Any(rt => rt.Reservation.StartTime < range.End && rt.Reservation.EndTime > range.Start));

            var tablesStatus = restaurantTables
                .Select(t => new TableStatus {
                    Table = _mapper.Map<TableModel>(t),
                    IsAvailable = availableTables.Contains(t)
                })
                .ToList();

            return tablesStatus;
        }
    }
}
