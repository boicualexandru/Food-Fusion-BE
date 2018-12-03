using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Authorization;
using Services.Employees;
using Services.Employees.Exceptions;
using WebApi.ActionFilters;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/Restaurants/{restaurantId}")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        // GET: api/Restaurants/5/Employees
        [AuthorizeByRestaurant(roles: "Employee")]
        [HttpGet("Employees")]
        public IActionResult GetEmployees(int restaurantId)
        {
            var employees = _employeesService.GetEmployees(restaurantId);
            return Ok(employees);
        }

        // POST: api/Restaurants/5/Employees
        [AuthorizeByRestaurant(roles: "Manager")]
        [HttpPost("Employees")]
        public IActionResult PostEmployee(int restaurantId, [FromBody] int userId)
        {
            var employee = _employeesService.AddEmployee(restaurantId, userId);
            return Ok(employee);
        }

        // DELETE: api/Restaurants/5/Employees/3
        [AuthorizeByRestaurant(roles: "Manager")]
        [HttpDelete("Employees/{userId}")]
        public IActionResult DeleteEmployee(int restaurantId, int userId)
        {
            try
            {
                _employeesService.RemoveEmployee(restaurantId, userId);
                return Ok();
            }
            catch (EmployeeNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Restaurants/5/Manager
        [AuthorizeByRestaurant(roles: "Employee")]
        [HttpGet("Manager")]
        public IActionResult GetManager(int restaurantId)
        {
            try
            {
                var manager = _employeesService.GetManager(restaurantId);
                return Ok(manager);
            }
            catch (ManagerNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Restaurants/5/Manager
        [AuthorizeByRestaurant(roles: "Manager")]
        [HttpPost("Manager")]
        public IActionResult PostManager(int restaurantId, [FromBody] int userId)
        {
            var manager = _employeesService.AddOrReplaceManager(restaurantId, userId);
            return Ok(manager);
        }
    }
}
