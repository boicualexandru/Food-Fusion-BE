using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using DataAccess.Models;
using Services.Reservations.Models;

namespace Services.Reservations
{
    public class ConcurrentEventsService : IConcurrentEventsService
    {
        public List<ConcurrentEvent> GetConcurrentEvents(List<Reservation> reservations)
        {
            if (!reservations?.Any() ?? true) return new List<ConcurrentEvent>();

            var orderedReservations = reservations
                .OrderBy(r => r.StartTime)
                .ThenBy(r => r.EndTime)
                .ToList();

            var allConcurrentReservations = new List<ConcurrentEvent>();

            var unhandeledReservations = new List<Reservation>(orderedReservations);
            var ongoingReservations = new List<Reservation>();
            var intervalStartTime = unhandeledReservations
                .FirstOrDefault()?.StartTime ?? default(DateTime);

            while (unhandeledReservations.Any() || ongoingReservations.Any())
            {
                var concurrentReservations = new List<Reservation>();
                var intervalEndTime = default(DateTime);

                // reorder ongoing reservations
                ongoingReservations = ongoingReservations.OrderBy(r => r.EndTime).ToList();

                var firstOngoing = ongoingReservations.FirstOrDefault();
                var firstUnhandled = unhandeledReservations.FirstOrDefault();

                if (firstUnhandled == null ||
                    firstOngoing?.EndTime < firstUnhandled?.StartTime)
                {
                    intervalEndTime = firstOngoing.EndTime;


                    allConcurrentReservations.Add(new ConcurrentEvent
                    {
                        TimeRange = new TimeRange
                        {
                            Start = intervalStartTime,
                            End = intervalEndTime
                        },
                        Events = ongoingReservations
                    });

                    var ongoingWithSameEnd = ongoingReservations
                        .TakeWhile(r => r.EndTime == intervalEndTime).ToList();

                    // take the left ones
                    ongoingReservations = ongoingReservations
                        .Except(ongoingWithSameEnd).ToList();

                    intervalStartTime = intervalEndTime;

                    continue;
                }

                var sameStartReservations = unhandeledReservations
                    .TakeWhile(r => r.StartTime == intervalStartTime).ToList();
                var firstOfSameStart = sameStartReservations.FirstOrDefault();

                unhandeledReservations = unhandeledReservations
                    .Except(sameStartReservations).ToList();
                firstUnhandled = unhandeledReservations.FirstOrDefault();

                if (firstUnhandled == null) intervalEndTime = firstOfSameStart?.EndTime ?? default(DateTime);
                if (firstOfSameStart == null) intervalEndTime = firstUnhandled?.StartTime ?? default(DateTime);

                if(firstUnhandled != null && firstOfSameStart != null)
                {
                    intervalEndTime = firstUnhandled?.StartTime < firstOfSameStart?.EndTime ?
                    firstUnhandled.StartTime : firstOfSameStart.EndTime;
                }
                
                if(firstOngoing != null)
                {
                    if (intervalEndTime == default(DateTime)) intervalEndTime = firstOngoing.EndTime;
                    else intervalEndTime = firstOngoing.EndTime < intervalEndTime ?
                        firstOngoing.EndTime : intervalEndTime;
                }

                // TODO: rethink -> it gets empty. Check start time
                var sameStartThatAreEnding = sameStartReservations
                    .TakeWhile(r => r.EndTime == intervalEndTime).ToList();

                var ongoingThatAreEnding = ongoingReservations
                    .TakeWhile(r => r.EndTime == intervalEndTime).ToList();

                allConcurrentReservations.Add(new ConcurrentEvent
                {
                    TimeRange = new TimeRange
                    {
                        Start = intervalStartTime,
                        End = intervalEndTime
                    },
                    Events = sameStartReservations
                        .Concat(ongoingReservations).ToList()
                });

                // take the left ones
                ongoingReservations = ongoingReservations
                    .Except(ongoingThatAreEnding).ToList();
                ongoingReservations.AddRange(sameStartReservations
                    .Except(sameStartThatAreEnding));

                intervalStartTime = intervalEndTime;

                continue;
            }

            return allConcurrentReservations;
        }
    }
}
