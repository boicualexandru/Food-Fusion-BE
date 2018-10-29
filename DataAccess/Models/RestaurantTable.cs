using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class RestaurantTable
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RestaurantMapId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public int Seats { get; set; }


        public virtual RestaurantMap RestaurantMap { get; set; }
        public virtual ICollection<ReservationTable> ReservationTables { get; set; }
    }
}
