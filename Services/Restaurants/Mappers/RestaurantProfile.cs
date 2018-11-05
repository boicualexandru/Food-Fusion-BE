using AutoMapper;
using DataAccess.Models;
using Services.Restaurants.Models;

namespace Services.Restaurants.Mappers
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Restaurant, RestaurantModel>();
            CreateMap<RestaurantModel, Restaurant>();
            
            CreateMap<Restaurant, RestaurantDetailedModel>();
            CreateMap<RestaurantDetailedModel, Restaurant>();
        }
    }
}
