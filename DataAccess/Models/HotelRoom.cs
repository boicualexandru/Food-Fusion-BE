using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class HotelRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public int Floor { get; set; }

        [Required]
        public int Beds { get; set; }

        [Required]
        public int MaxGuests { get; set; }
        
        [MaxLength(400)]
        public string ImageUrl { get; set; }

        public decimal Price { get; set; }


        public virtual ICollection<HotelRoomFeature> RoomFeatures { get; set; }
        public virtual ICollection<HotelRoomReservation> Reservations { get; set; }
    }
}
