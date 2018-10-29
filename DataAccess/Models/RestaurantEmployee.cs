using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class RestaurantEmployee
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public int UserId { get; set; }


        public virtual Restaurant Restaurant { get; set; }
        public virtual User User { get; set; }
    }
}
