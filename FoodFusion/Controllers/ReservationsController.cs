using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Authorization.Exceptions;
using Services.Reservations;
using Services.Reservations.Exceptions;
using Services.Reservations.Models;

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
        [HttpGet("Restaurants/{restaurantId}/Unavailability")]
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

        // POST: api/Restaurant/5/Reservations
        [HttpPost("Restaurants/{restaurantId}/Reservations")]
        public IActionResult Post([FromRoute] int restaurantId, [FromBody] ReservationRequestModel reservationRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) throw new InvalidClaimException();

            reservationRequest.UserId = userId;
            reservationRequest.RestaurantId = restaurantId;
            
            var reservation = _reservationsService.AddReservation(reservationRequest);
            return Ok(reservation);
        }

        // POST: api/Restaurant/5/Reservations
        [HttpPut("Reservations/{id}")]
        public IActionResult Put(int id, [FromBody] ReservationRequestModel reservationRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            reservationRequest.Id = id;

            try
            {
                var reservation = _reservationsService.UpdateReservation(reservationRequest);
                return Ok(reservation);
            }
            catch (ReservationNotFoundException)
            {
                return NotFound();
            }
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
