using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Authorization;
using Services.Employees;
using Services.Employees.Exceptions;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/Restaurants/{restaurantId}")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IResourceAuthorizationService<EmployeeAuthorizationRequirement> _authorizationService;
        private readonly IEmployeesService _employeesService;

        public EmployeesController(
            IResourceAuthorizationService<EmployeeAuthorizationRequirement> authorizationService,
            IEmployeesService employeesService)
        {
            _authorizationService = authorizationService;
            _employeesService = employeesService;
        }

        // GET: api/Restaurants/5/Employees
        [Authorize]
        [HttpGet("Employees")]
        public IActionResult GetEmployees(int restaurantId)
        {
            var isAuthorized = _authorizationService
                .WithUser(User)
                .WithRequirement(Operations<EmployeeAuthorizationRequirement>.Read)
                .WithResource(restaurantId)
                .IsAuthorized();
            if (!isAuthorized) return Forbid();

            var employees = _employeesService.GetEmployees(restaurantId);
            return Ok(employees);
        }

        // POST: api/Restaurants/5/Employees
        [Authorize]
        [HttpPost("Employees")]
        public IActionResult PostEmployee(int restaurantId, [FromBody] int userId)
        {
            var isAuthorized = _authorizationService
                .WithUser(User)
                .WithRequirement(Operations<EmployeeAuthorizationRequirement>.Create)
                .WithResource(restaurantId)
                .IsAuthorized();
            if (!isAuthorized) return Forbid();

            var employee = _employeesService.AddEmployee(restaurantId, userId);
            return Ok(employee);
        }

        // DELETE: api/Restaurants/5/Employees/3
        [Authorize]
        [HttpDelete("Employees/{userId}")]
        public IActionResult DeleteEmployee(int restaurantId, int userId)
        {
            try
            {
                var isAuthorized = _authorizationService
                    .WithUser(User)
                    .WithRequirement(Operations<EmployeeAuthorizationRequirement>.Delete)
                    .WithResource(restaurantId)
                    .IsAuthorized();
                if (!isAuthorized) return Forbid();

                _employeesService.RemoveEmployee(restaurantId, userId);
                return Ok();
            }
            catch (EmployeeNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Restaurants/5/Manager
        [Authorize]
        [HttpGet("Manager")]
        public IActionResult GetManager(int restaurantId)
        {
            try
            {
                var isAuthorized = _authorizationService
                    .WithUser(User)
                    .WithRequirement(Operations<EmployeeAuthorizationRequirement>.Read)
                    .WithResource(restaurantId)
                    .IsAuthorized();
                if (!isAuthorized) return Forbid();

                var manager = _employeesService.GetManager(restaurantId);
                return Ok(manager);
            }
            catch (ManagerNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Restaurants/5/Manager
        [Authorize]
        [HttpPost("Manager")]
        public IActionResult PostManager(int restaurantId, [FromBody] int userId)
        {
            var isAuthorized = _authorizationService
                .WithUser(User)
                .WithRequirement(Operations<EmployeeAuthorizationRequirement>.Create)
                .WithResource(restaurantId)
                .IsAuthorized();
            if (!isAuthorized) return Forbid();

            var manager = _employeesService.AddOrReplaceManager(restaurantId, userId);
            return Ok(manager);
        }
    }
}
