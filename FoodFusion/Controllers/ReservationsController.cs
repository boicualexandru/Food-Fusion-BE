using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Reservations;
using Services.Reservations.Exceptions;

namespace WebApi.Controllers
{
    //TODO: Add Authorization
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

        // GET: api/Restaurants/5/unavailability
        [HttpGet("Restaurants/{restaurantId}/unavailability")]
        public IActionResult GetUnavailability(
            [FromRoute] int restaurantId, 
            [FromQuery] int participantsCount, 
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var timeRange = new TimeRange { Start = start, End = end };

            var unavailability = _availabilityService
                .GetUnavailableTimeRanges(restaurantId, participantsCount, timeRange);

            return Ok(unavailability);
        }

        // GET: api/Reservations/5
        [HttpGet("Reservations/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var reservation = _reservationsService.GetReservation(id);

                return Ok(reservation);
            }
            catch (ReservationNotFoundException)
            {
                return NotFound();
            }
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

        // DELETE: api/Reservations/5
        [HttpDelete("Reservations/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _reservationsService.RemoveReservation(id);

                return Ok();
            }
            catch (ReservationNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
