using System.Collections.Generic;

namespace Common.ConcurrentEvents
{
    public class ConcurrentEvent
    {
        public TimeRange Range { get; set; }

        public List<IEvent> Events { get; set; }
    }
}
