using Services.Restaurants.Models;
using System.Collections.Generic;

namespace Services.Restaurants
{
    public interface IRestaurantService
    {
        int AddRestaurant(RestaurantInsertModel restaurantInsertModel, int managerUserId);
        IList<RestaurantModel> GetRestaurants(string city);
        RestaurantDetailedModel GetRestaurant(int id);
        void UpdateRestaurant(RestaurantModel restaurant);
        void DeleteRestaurant(int id);
    }
}
