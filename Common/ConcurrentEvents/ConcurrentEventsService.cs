using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ConcurrentEvents
{
    public class ConcurrentEventsService : IConcurrentEventsService
    {
        public List<ConcurrentEvent> GetConcurrentEvents(IEnumerable<IEvent> reservations)
        {
            if (!reservations?.Any() ?? true) return new List<ConcurrentEvent>();

            var orderedReservations = reservations
                .OrderBy(r => r.Range.Start)
                .ThenBy(r => r.Range.End)
                .ToList();

            var allConcurrentReservations = new List<ConcurrentEvent>();

            var unhandeledReservations = new List<IEvent>(orderedReservations);
            var ongoingReservations = new List<IEvent>();
            var intervalStartTime = unhandeledReservations
                .FirstOrDefault()?.Range.Start ?? default(DateTime);

            while (unhandeledReservations.Any() || ongoingReservations.Any())
            {
                var concurrentReservations = new List<IEvent>();
                var intervalEndTime = default(DateTime);

                // reorder ongoing reservations
                ongoingReservations = ongoingReservations.OrderBy(r => r.Range.End).ToList();

                var firstOngoing = ongoingReservations.FirstOrDefault();
                var firstUnhandled = unhandeledReservations.FirstOrDefault();

                if (firstUnhandled == null ||
                    firstOngoing?.Range.End < firstUnhandled?.Range.Start)
                {
                    intervalEndTime = firstOngoing.Range.End;


                    allConcurrentReservations.Add(new ConcurrentEvent
                    {
                        Range = new TimeRange
                        {
                            Start = intervalStartTime,
                            End = intervalEndTime
                        },
                        Events = ongoingReservations
                    });

                    var ongoingWithSameEnd = ongoingReservations
                        .TakeWhile(r => r.Range.End == intervalEndTime).ToList();

                    // take the left ones
                    ongoingReservations = ongoingReservations
                        .Except(ongoingWithSameEnd).ToList();

                    intervalStartTime = intervalEndTime;

                    continue;
                }

                var sameStartReservations = unhandeledReservations
                    .TakeWhile(r => r.Range.Start == intervalStartTime).ToList();
                var firstOfSameStart = sameStartReservations.FirstOrDefault();

                unhandeledReservations = unhandeledReservations
                    .Except(sameStartReservations).ToList();
                firstUnhandled = unhandeledReservations.FirstOrDefault();

                if (firstUnhandled == null) intervalEndTime = firstOfSameStart?.Range.End ?? default(DateTime);
                if (firstOfSameStart == null) intervalEndTime = firstUnhandled?.Range.Start ?? default(DateTime);

                if(firstUnhandled != null && firstOfSameStart != null)
                {
                    intervalEndTime = firstUnhandled?.Range.Start < firstOfSameStart?.Range.End ?
                    firstUnhandled.Range.Start : firstOfSameStart.Range.End;
                }
                
                if(firstOngoing != null)
                {
                    if (intervalEndTime == default(DateTime)) intervalEndTime = firstOngoing.Range.End;
                    else intervalEndTime = firstOngoing.Range.End < intervalEndTime ?
                        firstOngoing.Range.End : intervalEndTime;
                }

                // TODO: rethink -> it gets empty. Check start time
                var sameStartThatAreEnding = sameStartReservations
                    .TakeWhile(r => r.Range.End == intervalEndTime).ToList();

                var ongoingThatAreEnding = ongoingReservations
                    .TakeWhile(r => r.Range.End == intervalEndTime).ToList();

                allConcurrentReservations.Add(new ConcurrentEvent
                {
                    Range = new TimeRange
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
