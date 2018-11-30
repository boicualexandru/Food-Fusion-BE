using System.Collections.Generic;

namespace Common.ConcurrentEvents
{
    public class ConcurrentEvents<TEvent> where TEvent : IEvent
    {
        public TimeRange Range { get; set; }

        public List<TEvent> Events { get; set; }
    }
}
