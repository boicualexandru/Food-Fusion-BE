using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Reservations.Models
{
    public class ReservationRequestModel
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public int UserId { get; set; }

        [Required]
        public int? ParticipantsCount { get; set; }

        [Required]
        public TimeRange Range { get; set; }

        public List<int> TableIds { get; set; }
    }
}
