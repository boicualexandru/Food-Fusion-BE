using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DataAccess.Models;
using Services.Restaurants.Models;

namespace Services.Restaurants
{
    public class RestaurantService : IRestaurantService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IMapper _mapper;

        public RestaurantService(FoodFusionContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public RestaurantModel AddRestaurant(RestaurantModel restaurantModel)
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantModel);

            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return _mapper.Map<RestaurantModel>(restaurant);
        }
    }
}
