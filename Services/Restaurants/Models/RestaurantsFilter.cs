using DataAccess.Models;
using System.Collections.Generic;

namespace Services.Restaurants.Models
{
    public class RestaurantsFilter
    {
        public string SearchText { get; set; }

        public List<int> CuisineIds { get; set; }

        public List<PriceRange> PriceRanges { get; set; }
    }
}
