using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Authorization;
using Services.Restaurants;
using Services.Restaurants.Exceptions;
using Services.Restaurants.Models;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    /// <summary>
    /// Restaurant oriented operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IResourceAuthorizationService<RestaurantAuthorizationRequirement> _authorizationService;
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(
            IResourceAuthorizationService<RestaurantAuthorizationRequirement> authorizationService,
            IRestaurantService restaurantService)
        {
            _authorizationService = authorizationService;
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
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var restaurant = _restaurantService.GetRestaurant(id);
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
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RestaurantModel restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isAuthorized = _authorizationService
                   .WithUser(User)
                   .WithRequirement(Operations<RestaurantAuthorizationRequirement>.Update)
                   .WithResource(id)
                   .IsAuthorized();
                if (!isAuthorized) return Forbid();

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
