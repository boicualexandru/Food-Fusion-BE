using AutoMapper;
using DataAccess.Models;
using Services.Restaurants.Models;
using System.Linq;

namespace Services.Restaurants.Mappers
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Restaurant, RestaurantModel>()
                .ForMember(model => model.Cuisines,
                    opt => opt.MapFrom(restaurant => restaurant.RestaurantCuisines.Select(rc => rc.Cuisine)));
            CreateMap<RestaurantModel, Restaurant>()
                .ForMember(restaurant => restaurant.Id, opt => opt.Ignore());

            CreateMap<RestaurantInsertModel, Restaurant>()
                .ForMember(restaurant => restaurant.Id, opt => opt.Ignore())
                .ForMember(restaurant => restaurant.RestaurantCuisines, 
                    opt => opt.MapFrom(model => model.CuisineIds.Select(cuisineId => new RestaurantCuisine
                    {
                        CuisineId = cuisineId
                    })));

            CreateMap<Restaurant, RestaurantDetailedModel>()
                .ForMember(model => model.Cuisines,
                    opt => opt.MapFrom(restaurant => restaurant.RestaurantCuisines.Select(rc => rc.Cuisine)));
            CreateMap<RestaurantDetailedModel, Restaurant>()
                .ForMember(restaurant => restaurant.Id, opt => opt.Ignore());

            CreateMap<Restaurant, RestaurantSimpleModel>();
            CreateMap<RestaurantSimpleModel, Restaurant>()
                .ForMember(restaurant => restaurant.Id, opt => opt.Ignore());
        }
    }
}
