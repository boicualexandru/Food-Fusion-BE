using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class RestaurantCuisine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public int CuisineId { get; set; }


        public virtual Restaurant Restaurant { get; set; }
        public virtual Cuisine Cuisine { get; set; }
    }
}
