using Common;
using Services.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Hotel.Models
{
    public class HotelReservationDetailedModel
    {
        public int Id { get; set; }
        
        public virtual HotelRoomModel Room { get; set; }

        public UserSimpleModel User { get; set; }

        public int GuestsCount { get; set; }

        public TimeRange Range { get; set; }
    }
}
