using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Reservations;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationsService _reservationsService;
        private readonly IAvailabilityService _availabilityService;

        public ReservationsController(
            IReservationsService reservationsService, 
            IAvailabilityService availabilityService)
        {
            _reservationsService = reservationsService;
            _availabilityService = availabilityService;
        }

        // GET: api/Reservations
        [HttpGet("Restaurants/{restaurantId}/availability")]
        public IActionResult Get(
            [FromRoute] int restaurantId, 
            [FromQuery] int participantsCount, 
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var timeRange = new TimeRange { Start = start, End = end };

            var availability = _availabilityService
                .GetUnavailableTimeRanges(restaurantId, participantsCount, timeRange);

            return Ok(availability);
        }

        // GET: api/Reservations/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Reservations
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Reservations/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
