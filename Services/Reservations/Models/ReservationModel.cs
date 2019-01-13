using Common;
using Services.Restaurants.Models;
using Services.Users.Models;

namespace Services.Reservations.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }

        public RestaurantSimpleModel Restaurant { get; set; }

        public UserSimpleModel User { get; set; }

        public int ParticipantsCount { get; set; }

        public TimeRange Range { get; set; }
    }
}