using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class HotelFeature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Category { get; set; }
        
        [MaxLength(200)]
        public string Icon { get; set; }


        public virtual ICollection<HotelRoomFeature> RoomFeatures { get; set; }
    }
}
