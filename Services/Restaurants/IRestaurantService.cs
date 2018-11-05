using Services.Restaurants.Models;
using System.Collections.Generic;

namespace Services.Restaurants
{
    public interface IRestaurantService
    {
        RestaurantModel AddRestaurant(RestaurantModel restaurantModel);
        IList<RestaurantModel> GetRestaurants(string city);
        RestaurantDetailedModel GetRestaurant(int id);
    }
}
