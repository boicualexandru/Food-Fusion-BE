using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Authorization;
using Services.Restaurants;
using Services.Restaurants.Exceptions;
using Services.Restaurants.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
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
        [HttpGet]
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
                return BadRequest();
            }
        }

        // POST: api/Restaurants
        [Authorize("Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] RestaurantModel restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _restaurantService.AddRestaurant(restaurant);

            return Ok();
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

            var isAuthorized = _authorizationService
                .WithUser(User)
                .WithRequirement(Operations<RestaurantAuthorizationRequirement>.Update)
                .WithResource(id)
                .IsAuthorized();
            if (!isAuthorized) return Forbid();

            throw new NotImplementedException();
        }

        // DELETE: api/Restaurants/5
        [Authorize("Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
