using DataAccess.Models;
using Services.Reservations.Models;
using System.Collections.Generic;

namespace Services.Reservations
{
    public interface IConcurrentEventsService
    {
        List<ConcurrentEvent> GetConcurrentEvents(List<Reservation> reservations);
    }
}
