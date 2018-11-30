using Common;
using Common.ConcurrentEvents;
using Services.Restaurants.Models;
using Services.Users.Models;
using System.Collections.Generic;

namespace Services.Reservations.Models
{
    public class ReservationDetailedModel : IEvent
    {
        public int Id { get; set; }

        public RestaurantSimpleModel Restaurant { get; set; }

        public UserSimpleModel User { get; set; }

        public int ParticipantsCount { get; set; }

        public TimeRange Range { get; set; }

        public List<TableModel> Tables { get; set; }
    }
}