using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    /// <summary>
    /// Joining Entity between Restaurants and employed Users
    /// </summary>
    public class RestaurantEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int RestaurantId { get; set; }
        
        public int UserId { get; set; }


        public virtual Restaurant Restaurant { get; set; }
        public virtual User User { get; set; }
    }
}
