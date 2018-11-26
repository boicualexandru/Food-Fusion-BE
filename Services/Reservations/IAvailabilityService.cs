using Common;
using DataAccess.Models;
using System.Collections.Generic;

namespace Services.Reservations
{
    public interface IAvailabilityService
    {
        IList<TimeRange> GetUnavailableTimeRanges(int restaurantId, int participantsCount, TimeRange timeRange);

        bool AreTablesAvailable(IList<int> tableIds, TimeRange range, IList<int> reservedTableidsToExclude = null);

        bool AreTooManyTablesRequested(IList<RestaurantTable> tables, int participantsCount);
    }
}
