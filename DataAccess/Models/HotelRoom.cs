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


        public virtual ICollection<HotelRoomFeature> RoomFeatures { get; set; }
    }
}
