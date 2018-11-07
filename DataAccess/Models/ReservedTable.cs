using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    /// <summary>
    /// Joining Entity between Reservation and RestaurantTable
    /// </summary>
    public class ReservedTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int ReservationId { get; set; }
        
        public int RestaurantTableId { get; set; }


        public virtual Reservation Reservation { get; set; }
        public virtual RestaurantTable Table { get; set; }
    }
}
