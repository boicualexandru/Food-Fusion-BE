using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Restaurant
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Contact { get; set; }

        [MaxLength(200)]
        public string City { get; set; }
        
        public int? ManagerId { get; set; }
        
        public int? MenuId { get; set; }

        public int? RestaurantMapId { get; set; }


        public virtual User Manager { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual RestaurantMap Map { get; set; }
        public virtual ICollection<RestaurantEmployee> RestaurantEmployee { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
