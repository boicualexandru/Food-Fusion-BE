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
            CreateMap<RestaurantModel, Restaurant>()
                .ForMember(restaurant => restaurant.Id, opt => opt.Ignore());
            
            CreateMap<Restaurant, RestaurantDetailedModel>();
            CreateMap<RestaurantDetailedModel, Restaurant>()
                .ForMember(restaurant => restaurant.Id, opt => opt.Ignore());

            CreateMap<Restaurant, RestaurantSimpleModel>();
            CreateMap<RestaurantSimpleModel, Restaurant>()
                .ForMember(restaurant => restaurant.Id, opt => opt.Ignore());
        }
    }
}
