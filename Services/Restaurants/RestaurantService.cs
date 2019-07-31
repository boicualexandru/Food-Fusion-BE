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

        public int AddRestaurant(RestaurantInsertModel restaurantInsertModel, int managerUserId)
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantInsertModel);

            restaurant.ManagerId = managerUserId;
            restaurant.Menu = new Menu();
            restaurant.Map = new RestaurantMap();

            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public RestaurantDetailedModel GetRestaurant(int id)
        {
            var restaurant = _dbContext.Restaurants
                .AsNoTracking()
                .Include(r => r.Menu)
                    .ThenInclude(menu => menu.Items)
                .Include(r => r.RestaurantCuisines)
                    .ThenInclude(rc => rc.Cuisine)
                .FirstOrDefault(r => r.Id == id);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            return _mapper.Map<RestaurantDetailedModel>(restaurant);
        }

        public IList<RestaurantModel> GetRestaurants(RestaurantsFilter filter)
        {
            var restaurantsQuery = _dbContext.Restaurants
                .Include(r => r.RestaurantCuisines)
                    .ThenInclude(rc => rc.Cuisine)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                restaurantsQuery = restaurantsQuery
                    .Where(r => r.Name.Contains(filter.SearchText, StringComparison.InvariantCultureIgnoreCase));
            }

            if (filter.CuisineIds?.Any() == true)
            {
                restaurantsQuery = restaurantsQuery
                    .Where(r => r.RestaurantCuisines.Any(rc => filter.CuisineIds.Contains(rc.CuisineId)));
            }

            if (filter.PriceRanges?.Any() == true)
            {
                restaurantsQuery = restaurantsQuery
                    .Where(r => r.PriceRange.HasValue && filter.PriceRanges.Contains(r.PriceRange.Value));
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
