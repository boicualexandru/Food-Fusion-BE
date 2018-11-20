using System.Collections.Generic;

namespace Common.ConcurrentEvents
{
    public interface IConcurrentEventsService<TEvent> where TEvent : IEvent
    {
        List<ConcurrentEvents<TEvent>> GetConcurrentEvents(IEnumerable<TEvent> reservations);
    }
}
