using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Authorization.Exceptions;
using Services.Hotel;
using Services.Hotel.Models;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet("Features")]
        public IActionResult GetFeatures()
        {
            var features = _hotelService.GetAvailableFeatures();

            return Ok(features);
        }

        [HttpPost("Rooms")]
        public IActionResult GetRooms(HotelRoomsFiltersModel filters)
        {
            var rooms = _hotelService.GetRooms(filters);

            return Ok(rooms);
        }

        [Authorize]
        [HttpPost("Reservations")]
        public IActionResult BookRoom(HotelRoomBookingModel bookingDetails)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) throw new InvalidClaimException();

            bookingDetails.UserId = userId;

            _hotelService.BookRoom(bookingDetails);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Reservations")]
        public IActionResult GetAllReservations()
        {
            var reservations = _hotelService.GetReservations();
            return Ok(reservations);
        }

        [Authorize]
        [HttpGet("UserReservations")]
        public IActionResult GetUserReservations()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) throw new InvalidClaimException();

            var reservations = _hotelService.GetReservations(userId);
            return Ok(reservations);
        }

        // PUT: api/Reservations/5
        [AuthorizeByHotelReservation(roles: "Owner")]
        [HttpPut("PayReservation/{id}")]
        public IActionResult MarkReservationAsPaid(int id)
        {
            _hotelService.MarkReservationAsPaid(id);

            return Ok();
        }
    }
}