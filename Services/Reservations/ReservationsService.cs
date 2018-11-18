using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Common;
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

        public ReservationsService(FoodFusionContext dbContext, IMapper mapper)
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

        public IList<TimeRange> GetAvailability(int restaurantId, int participantsCount, TimeRange timeRange)
        {
            var restaurant = _dbContext.Restaurants
                .AsNoTracking()
                .Include(r => r.Map)
                    .ThenInclude(m => m.Tables)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            var tables = restaurant.Map.Tables;

            // TODO: improve this to be done in one query
            bool isReservationInRange(Reservation res) => res.EndTime > timeRange.Start ||
                    res.StartTime > timeRange.End;

            // use service
            // bool isReservationABlocker(Reservation res) => res

            var reservations = _dbContext.Reservations
                .Include(r => r.ReservedTables)
                    .ThenInclude(rt => rt.Table)
                .Where(r => r.RestaurantId == restaurantId)
                .Where(isReservationInRange);

            throw new NotImplementedException();
        }


        //private List<ConcurrentEvent> GetConcurrentReservationsPeriods(List<Reservation> reservations)
        //{
        //    if (!reservations.Any()) return new List<ConcurrentEvent>();

        //    var orderedReservations = reservations
        //        .OrderBy(r => r.StartTime)
        //        .ThenBy(r => r.EndTime)
        //        .ToList();

        //    var overlappingReservationsPeriods = new List<ConcurrentEvent>();

        //    var unhandeledReservations = new List<Reservation>(orderedReservations);
        //    var ongoingReservations = new List<Reservation>();
        //    var intervalStartTime = unhandeledReservations
        //        .FirstOrDefault()?.StartTime ?? DateTime.MinValue;

        //    while(unhandeledReservations.Any() || ongoingReservations.Any())
        //    {
        //        var concurrentReservations = new List<Reservation>();
        //        DateTime intervalEndTime;

        //        // reorder ongoing reservations
        //        ongoingReservations = ongoingReservations.OrderBy(r => r.EndTime).ToList();

        //        var firstOngoing = ongoingReservations.FirstOrDefault();
        //        var firstUnhandled = unhandeledReservations.FirstOrDefault();

        //        if (firstUnhandled == null || 
        //            firstOngoing?.EndTime < firstUnhandled?.StartTime)
        //        {
        //            intervalEndTime = firstOngoing.EndTime;

        //            var ongoingWithSameEnd = ongoingReservations
        //                .TakeWhile(r => r.EndTime == intervalEndTime).ToList();
                    
        //            overlappingReservationsPeriods.Add(new ConcurrentEvent {
        //                TimeRange = new TimeRange
        //                {
        //                    Start = intervalStartTime,
        //                    End = intervalEndTime
        //                },
        //                Reservations = ongoingWithSameEnd
        //            });

        //            // take the left ones
        //            ongoingReservations = ongoingReservations
        //                .Except(ongoingWithSameEnd).ToList();

        //            intervalStartTime = intervalEndTime;

        //            continue;
        //        }

        //        var sameStartReservations = unhandeledReservations
        //            .TakeWhile(r => r.StartTime == intervalStartTime).ToList();
        //        var firstOfSameStart = sameStartReservations.FirstOrDefault();

        //        unhandeledReservations = unhandeledReservations
        //            .Except(sameStartReservations).ToList();
        //        firstUnhandled = unhandeledReservations.FirstOrDefault();

        //        intervalEndTime = firstUnhandled?.StartTime < firstOfSameStart?.EndTime ?
        //            firstUnhandled?.StartTime ?? DateTime.MinValue :
        //            firstOfSameStart?.EndTime ?? DateTime.MinValue;

        //        var sameStartAndSameEnd = sameStartReservations
        //            .TakeWhile(r => r.EndTime == intervalEndTime).ToList();

        //        overlappingReservationsPeriods.Add(new ConcurrentEvent
        //        {
        //            TimeRange = new TimeRange
        //            {
        //                Start = intervalStartTime,
        //                End = intervalEndTime
        //            },
        //            Reservations = sameStartAndSameEnd
        //        });

        //        // take the left ones
        //        ongoingReservations = sameStartReservations
        //            .Except(sameStartAndSameEnd).ToList();

        //        intervalStartTime = intervalEndTime;

        //        continue;
        //    }

        //    return overlappingReservationsPeriods;
        //}


        //// GetConcurrentReservationsPeriod
        //private List<ConcurrentEvent> GetOverlappingReservations(List<Reservation> reservation)
        //{
        //    // TODO check not empty

        //    var orderedReservations = reservations
        //        .OrderBy(r => r.StartTime)
        //        .ThenBy(r => r.EndTime)
        //        .ToList();

        //    var overlappingReservationsPeriods = new List<ConcurrentEvent>();

        //    var inheritedReservations = new List<Reservation>();
        //    //foreach interval
        //    var concurencyPeriodStart = orderedReservations[0].StartTime;

        //    var concurrentReservations = new List<Reservation>();

        //    var i = 0;
        //    while (orderedReservations[i].StartTime == orderedReservations[0].StartTime &&
        //        i < orderedReservations.Count())
        //    {
        //        concurrentReservations.Add(orderedReservations[i]);
        //        i++;
        //    }

        //    var indexOfLastConcurrentReservationWithTheSameEndAsFirst =
        //        concurrentReservations.FindLastIndex(r => r.EndTime == concurrentReservations[0].EndTime);

        //    // take all
        //    inheritedReservations.AddRange(
        //        concurrentReservations
        //            .Skip(indexOfLastConcurrentReservationWithTheSameEndAsFirst + 1));

        //    //in debd reservations = without the first

        //    var isEndOfList = i < orderedReservations.Count()
        //    if (i < orderedReservations.Count()) {
        //        // the lowest between: 
        //        // [end time of those with same start interval], 
        //        // [end time of those inherited]
        //        // [start time of the next one in list]
        //        var concurencyPeriodEnd = new DateTime(Math.Min(
        //            orderedReservations[0].EndTime.Ticks,
        //            orderedReservations[i].StartTime.Ticks));
        //    }
                







        //    concurencyPeriodStart = concurencyPeriodEnd;




        //    for (var index = 0; index < orderedReservations.Count(); index++)
        //    {
        //        var concurrentReservations = new List<Reservation>();

        //        var j = index;
        //        while(orderedReservations[j].StartTime == orderedReservations[index].StartTime &&
        //            j < orderedReservations.Count())
        //        {
        //            concurrentReservations.Add(orderedReservations[j]);
        //            j++;
        //        }

        //        var
        //    }
        //}

        //private class ConcurrentEvent
        //{
        //    public TimeRange TimeRange { get; set; }

        //    public List<Reservation> Reservations { get; set; }
        //}

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
