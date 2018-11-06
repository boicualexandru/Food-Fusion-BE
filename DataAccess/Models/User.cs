using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public string HashPassword { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }
        
        public UserRole Role { get; set; }


        public virtual ICollection<Restaurant> ManagedRestaurants { get; set; }
        public virtual ICollection<RestaurantEmployee> RestaurantsEmployee { get; set; }
    }
}
