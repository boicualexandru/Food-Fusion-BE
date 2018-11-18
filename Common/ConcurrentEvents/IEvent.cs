using Common;

namespace Common.ConcurrentEvents
{
    public interface IEvent
    {
        TimeRange Range { get; set; }
    }
}
