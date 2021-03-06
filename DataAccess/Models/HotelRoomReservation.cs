﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class HotelRoomReservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int HotelRoomId { get; set; }

        public int UserId { get; set; }

        public int GuestsCount { get; set; }

        public bool Paid { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }


        public virtual HotelRoom Room { get; set; }
        public virtual User User { get; set; }
    }
}
