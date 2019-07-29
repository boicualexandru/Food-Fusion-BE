using Microsoft.AspNetCore.Mvc;
using Services.Cuisines;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisinesController : ControllerBase
    {
        private readonly ICuisinesService _cuisinesService;

        public CuisinesController(ICuisinesService cuisinesService)
        {
            _cuisinesService = cuisinesService;
        }

        // GET: api/Cuisines
        [HttpGet]
        public IActionResult GetCuisines()
        {
            var cuisines = _cuisinesService.GetCuisines();
            return Ok(cuisines);
        }
    }
}