using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Restaurants.Exceptions;
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

        public RestaurantModel AddRestaurant(RestaurantModel restaurantModel, int managerUserId)
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantModel);

            restaurant.ManagerId = managerUserId;

            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return _mapper.Map<RestaurantModel>(restaurant);
        }

        public RestaurantDetailedModel GetRestaurant(int id)
        {
            var restaurant = _dbContext.Restaurants
                .AsNoTracking()
                .Include(r => r.Menu)
                    .ThenInclude(menu => menu.Items)
                .FirstOrDefault(r => r.Id == id);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            return _mapper.Map<RestaurantDetailedModel>(restaurant);
        }

        public IList<RestaurantModel> GetRestaurants(string city)
        {
            var restaurantsQuery = _dbContext.Restaurants
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(city))
            {
                restaurantsQuery = restaurantsQuery
                    .Where(r => string.Equals(r.City, city.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            
            var restaurants = restaurantsQuery.ToList();

            return _mapper.Map<IList<RestaurantModel>>(restaurants);
        }

        public void UpdateRestaurant(RestaurantModel restaurantModel)
        {
            var restaurant = _dbContext.Restaurants
                .FirstOrDefault(r => r.Id == restaurantModel.Id);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            restaurant = _mapper.Map(restaurantModel, restaurant);

            _dbContext.SaveChanges();
        }

        public void DeleteRestaurant(int id)
        {
            var restaurant = _dbContext.Restaurants
                .FirstOrDefault(r => r.Id == id);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();

            return;
        }
    }
}
