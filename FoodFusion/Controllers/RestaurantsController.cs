﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Restaurants;
using Services.Restaurants.Exceptions;
using Services.Restaurants.Models;
using System.Collections.Generic;
using WebApi.ActionFilters;

namespace WebApi.Controllers
{
    /// <summary>
    /// Restaurant oriented operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        // GET: api/Restaurants
        /// <summary>
        /// Returns a list of restaurants filtered by city
        /// </summary>
        /// <param name="city">Optiona: City to be filter by</param>
        /// <returns>List of restaurants</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<RestaurantModel>), StatusCodes.Status200OK)]
        public IActionResult Get(string city)
        {
            var restaurants = _restaurantService.GetRestaurants(city);

            return Ok(restaurants);
        }

        // GET: api/Restaurants/5
        [HttpGet("{menuId}")]
        public IActionResult Get(int menuId)
        {
            try
            {
                var restaurant = _restaurantService.GetRestaurant(menuId);
                return Ok(restaurant);
            }
            catch(RestaurantNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Restaurants
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] RestaurantModel restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRestaurant = _restaurantService.AddRestaurant(restaurant);

            return Ok(createdRestaurant);
        }

        // PUT: api/Restaurants/5
        [AuthorizeByRestaurant(roles: "Admin, Manager", key: "id")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RestaurantModel restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                restaurant.Id = id;
                _restaurantService.UpdateRestaurant(restaurant);
                return Ok(restaurant);
            }
            catch (RestaurantNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Restaurants/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _restaurantService.DeleteRestaurant(id);
                return Ok();
            }
            catch (RestaurantNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
