using System.Collections.Generic;

namespace Common.ConcurrentEvents
{
    public interface IConcurrentEventsService
    {
        List<ConcurrentEvent> GetConcurrentEvents(IEnumerable<IEvent> reservations);
    }
}
