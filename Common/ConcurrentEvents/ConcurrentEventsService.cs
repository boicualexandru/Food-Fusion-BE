using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ConcurrentEvents
{
    public class ConcurrentEventsService<TEvent> : IConcurrentEventsService<TEvent> where TEvent : IEvent
    {
        public List<ConcurrentEvent<TEvent>> GetConcurrentEvents(IEnumerable<TEvent> events)
        {
            if (!events?.Any() ?? true) return new List<ConcurrentEvent<TEvent>>();

            var orderedEvents = events
                .OrderBy(e => e.Range.Start)
                .ThenBy(e => e.Range.End)
                .ToList();

            var allConcurrentEvents = new List<ConcurrentEvent<TEvent>>();

            var unhandeledEvents = new List<TEvent>(orderedEvents);
            var ongoingEvents = new List<TEvent>();
            var intervalStartTime = unhandeledEvents
                .FirstOrDefault()?.Range.Start ?? default;

            while (unhandeledEvents.Any() || ongoingEvents.Any())
            {
                var concurrentEvents = new List<IEvent>();
                DateTime intervalEndTime = default;

                // reorder ongoing events
                ongoingEvents = ongoingEvents.OrderBy(e => e.Range.End).ToList();

                var firstOngoing = ongoingEvents.FirstOrDefault();
                var firstUnhandled = unhandeledEvents.FirstOrDefault();

                if (firstUnhandled == null ||
                    firstOngoing?.Range.End < firstUnhandled?.Range.Start)
                {
                    intervalEndTime = firstOngoing.Range.End;


                    allConcurrentEvents.Add(new ConcurrentEvent<TEvent>
                    {
                        Range = new TimeRange
                        {
                            Start = intervalStartTime,
                            End = intervalEndTime
                        },
                        Events = ongoingEvents
                    });

                    var ongoingWithSameEnd = ongoingEvents
                        .TakeWhile(e => e.Range.End == intervalEndTime).ToList();

                    // take the left ones
                    ongoingEvents = ongoingEvents
                        .Except(ongoingWithSameEnd).ToList();

                    intervalStartTime = intervalEndTime;

                    continue;
                }

                var sameStartEvents = unhandeledEvents
                    .TakeWhile(e => e.Range.Start == intervalStartTime).ToList();
                var firstOfSameStart = sameStartEvents.FirstOrDefault();

                unhandeledEvents = unhandeledEvents
                    .Except(sameStartEvents).ToList();
                firstUnhandled = unhandeledEvents.FirstOrDefault();

                if (firstUnhandled == null) intervalEndTime = firstOfSameStart?.Range.End ?? default;
                if (firstOfSameStart == null) intervalEndTime = firstUnhandled?.Range.Start ?? default;

                if(firstUnhandled != null && firstOfSameStart != null)
                {
                    intervalEndTime = firstUnhandled?.Range.Start < firstOfSameStart?.Range.End ?
                    firstUnhandled.Range.Start : firstOfSameStart.Range.End;
                }
                
                if(firstOngoing != null)
                {
                    if (intervalEndTime == default) intervalEndTime = firstOngoing.Range.End;
                    else intervalEndTime = firstOngoing.Range.End < intervalEndTime ?
                        firstOngoing.Range.End : intervalEndTime;
                }
                
                var sameStartThatAreEnding = sameStartEvents
                    .TakeWhile(e => e.Range.End == intervalEndTime).ToList();

                var ongoingThatAreEnding = ongoingEvents
                    .TakeWhile(e => e.Range.End == intervalEndTime).ToList();

                allConcurrentEvents.Add(new ConcurrentEvent<TEvent>
                {
                    Range = new TimeRange
                    {
                        Start = intervalStartTime,
                        End = intervalEndTime
                    },
                    Events = sameStartEvents
                        .Concat(ongoingEvents).ToList()
                });

                // take the left ones
                ongoingEvents = ongoingEvents
                    .Except(ongoingThatAreEnding).ToList();
                ongoingEvents.AddRange(sameStartEvents
                    .Except(sameStartThatAreEnding));

                intervalStartTime = intervalEndTime;

                continue;
            }

            return allConcurrentEvents;
        }
    }
}
