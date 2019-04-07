using Microsoft.AspNetCore.Mvc;
using Services.Hotel;
using Services.Hotel.Models;

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

        [HttpGet("features")]
        public IActionResult GetFeatures()
        {
            var features = _hotelService.GetAvailableFeatures();

            return Ok(features);
        }

        [HttpPost("rooms")]
        public IActionResult GetRooms(HotelRoomsFiltersModel filters)
        {
            var rooms = _hotelService.GetRooms(filters);

            return Ok(rooms);
        }
    }
}