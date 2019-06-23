using Services.Menus.Models;
using System.ComponentModel.DataAnnotations;

namespace Services.Restaurants.Models
{
    public class RestaurantDetailedModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Contact { get; set; }

        public string ImageUrl { get; set; }

        public string City { get; set; }

        public long GeoLatitude { get; set; }

        public long GeoLongitude { get; set; }

        public MenuModel Menu { get; set; }
    }
}