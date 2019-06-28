using Common;
using System.ComponentModel.DataAnnotations;

namespace Services.Hotel.Models
{
    public class HotelRoomBookingModel
    {
        [Required]
        public int RoomId { get; set; }

        public int UserId { get; set; }

        [Required]
        public TimeRange Range { get; set; }

        [Required]
        public int Guests { get; set; }
    }
}
