using Services.Restaurants.Models;

namespace Services.Restaurants
{
    public interface IRestaurantService
    {
        RestaurantModel AddRestaurant(RestaurantModel restaurantModel);
    }
}
