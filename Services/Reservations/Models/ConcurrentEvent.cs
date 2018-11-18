using Common;
using DataAccess.Models;
using System.Collections.Generic;

namespace Services.Reservations.Models
{
    public class ConcurrentEvent
    {
        public TimeRange TimeRange { get; set; }

        public List<Reservation> Reservations { get; set; }
    }
}
