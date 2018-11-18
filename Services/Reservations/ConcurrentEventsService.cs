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
                .FirstOrDefault()?.StartTime ?? DateTime.MinValue;

            while (unhandeledReservations.Any() || ongoingReservations.Any())
            {
                var concurrentReservations = new List<Reservation>();
                DateTime intervalEndTime;

                // reorder ongoing reservations
                ongoingReservations = ongoingReservations.OrderBy(r => r.EndTime).ToList();

                var firstOngoing = ongoingReservations.FirstOrDefault();
                var firstUnhandled = unhandeledReservations.FirstOrDefault();

                if (firstUnhandled == null ||
                    firstOngoing?.EndTime < firstUnhandled?.StartTime)
                {
                    intervalEndTime = firstOngoing.EndTime;

                    var ongoingWithSameEnd = ongoingReservations
                        .TakeWhile(r => r.EndTime == intervalEndTime).ToList();

                    allConcurrentReservations.Add(new ConcurrentEvent
                    {
                        TimeRange = new TimeRange
                        {
                            Start = intervalStartTime,
                            End = intervalEndTime
                        },
                        Reservations = ongoingWithSameEnd
                    });

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

                intervalEndTime = firstUnhandled?.StartTime < firstOfSameStart?.EndTime ?
                    firstUnhandled?.StartTime ?? DateTime.MinValue :
                    firstOfSameStart?.EndTime ?? DateTime.MinValue;

                // TODO: rethink -> it gets empty. Check start time
                var sameStartAndSameEnd = sameStartReservations
                    .TakeWhile(r => r.EndTime == intervalEndTime).ToList();

                allConcurrentReservations.Add(new ConcurrentEvent
                {
                    TimeRange = new TimeRange
                    {
                        Start = intervalStartTime,
                        End = intervalEndTime
                    },
                    Reservations = sameStartAndSameEnd
                });

                // take the left ones
                ongoingReservations = sameStartReservations
                    .Except(sameStartAndSameEnd).ToList();

                intervalStartTime = intervalEndTime;

                continue;
            }

            return allConcurrentReservations;
        }
    }
}
