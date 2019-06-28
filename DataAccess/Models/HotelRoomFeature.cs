using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class HotelRoomFeature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RoomId { get; set; }

        public int FeatureId { get; set; }


        public virtual HotelRoom Room { get; set; }
        public virtual HotelFeature Feature { get; set; }
    }
}
