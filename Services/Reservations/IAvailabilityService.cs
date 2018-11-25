using Common;
using System.Collections.Generic;

namespace Services.Reservations
{
    public interface IAvailabilityService
    {
        IList<TimeRange> GetUnavailableTimeRanges(int restaurantId, int participantsCount, TimeRange timeRange);

        bool AreTablesAvailable(IList<int> tableIds, TimeRange range);
    }
}
