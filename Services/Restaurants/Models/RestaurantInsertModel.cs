using DataAccess.Models;
using Services.Cuisines.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Restaurants.Models
{
    public class RestaurantInsertModel
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Contact { get; set; }

        public string ImageUrl { get; set; }

        public string City { get; set; }

        public double GeoLatitude { get; set; }

        public double GeoLongitude { get; set; }

        public PriceRange? PriceRange { get; set; }

        public List<int> CuisineIds { get; set; }
    }
}