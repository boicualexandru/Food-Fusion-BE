using System.Collections.Generic;

namespace Common.ConcurrentEvents
{
    public interface IConcurrentEventsService<TEvent> where TEvent : IEvent
    {
        List<ConcurrentEvent<TEvent>> GetConcurrentEvents(IEnumerable<TEvent> reservations);
    }
}
