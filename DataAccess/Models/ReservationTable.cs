using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class ReservationTable
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int RestaurantTableId { get; set; }


        public virtual Reservation Reservation { get; set; }
        public virtual RestaurantTable RestaurantTable { get; set; }
    }
}
