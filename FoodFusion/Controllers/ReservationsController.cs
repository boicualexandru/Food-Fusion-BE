using System;
using System.Security.Claims;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Authorization;
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
        private readonly IResourceAuthorizationService<ReservationAuthorizationRequirement> _reservationAuthorizationService;
        private readonly IReservationsService _reservationsService;
        private readonly IAvailabilityService _availabilityService;

        public ReservationsController(
            IResourceAuthorizationService<ReservationAuthorizationRequirement> reservationAuthorizationService,
            IReservationsService reservationsService, 
            IAvailabilityService availabilityService)
        {
            _reservationAuthorizationService = reservationAuthorizationService;
            _reservationsService = reservationsService;
            _availabilityService = availabilityService;
        }

        // GET: api/Restaurants/5/unavailability
        [Authorize]
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

        // POST: api/Restaurant/5/Reservations
        [Authorize]
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

        // GET: api/Restaurant/5/Reservations
        [Authorize]
        [HttpGet("Restaurant/{restaurantId}/Reservations")]
        public IActionResult GetRestaurantReservations(int restaurantId)
        {
            var reservations = _reservationsService.GetRestaurantReservations(restaurantId);

            return Ok(reservations);
        }

        // GET: api/Reservations
        [Authorize]
        [HttpGet("Reservations")]
        public IActionResult GetUserReservations()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) throw new InvalidClaimException();

            var reservations = _reservationsService.GetUserReservations(userId);

            return Ok(reservations);
        }

        // GET: api/Reservations/5
        [Authorize]
        [HttpGet("Reservations/{id}")]
        public IActionResult Get(int id)
        {
            var isAuthorized = _reservationAuthorizationService
                .WithUser(User)
                .WithRequirement(Operations<ReservationAuthorizationRequirement>.Read)
                .WithResource(id)
                .IsAuthorized();
            if (!isAuthorized) return Forbid();

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

        // PUT: api/Restaurant/5/Reservations
        [Authorize]
        [HttpPut("Reservations/{id}")]
        public IActionResult Put(int id, [FromBody] ReservationRequestModel reservationRequest)
        {
            var isAuthorized = _reservationAuthorizationService
                .WithUser(User)
                .WithRequirement(Operations<ReservationAuthorizationRequirement>.Update)
                .WithResource(id)
                .IsAuthorized();
            if (!isAuthorized) return Forbid();

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
        [Authorize]
        [HttpDelete("Reservations/{id}")]
        public IActionResult Delete(int id)
        {
            var isAuthorized = _reservationAuthorizationService
                .WithUser(User)
                .WithRequirement(Operations<ReservationAuthorizationRequirement>.Delete)
                .WithResource(id)
                .IsAuthorized();
            if (!isAuthorized) return Forbid();

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
