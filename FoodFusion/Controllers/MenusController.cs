using Microsoft.AspNetCore.Mvc;
using Services.Menus;
using Services.Menus.Exceptions;
using Services.Menus.Models;
using Services.Restaurants.Exceptions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/Menus
        [HttpGet("{restaurantId}")]
        public IActionResult Get(int restaurantId)
        {
            try
            {
                var menu = _menuService.GetMenu(restaurantId);
                return Ok(menu);
            }
            catch (RestaurantNotFoundException)
            {
                return BadRequest();
            }
            catch (MenuNotFoundException)
            {
                return BadRequest();
            }
        }

        // POST: api/Menus
        [HttpPost("{restaurantId}")]
        public IActionResult Post(int restaurantId, [FromBody] MenuModel menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdMenu = _menuService.AddMenuIfNotExists(restaurantId, menu);
                return Ok(createdMenu);
            }
            catch (RestaurantNotFoundException)
            {
                return BadRequest();
            }
            catch (MenuAlreadyExistsException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{restaurantId}")]
        public IActionResult Delete(int restaurantId)
        {
            try
            {
                _menuService.RemoveMenu(restaurantId);
                return Ok();
            }
            catch (RestaurantNotFoundException)
            {
                return BadRequest();
            }
            catch (MenuNotFoundException)
            {
                return BadRequest();
            }
        }

        // POST: api/Menus
        [HttpPost("{menuId}/items")]
        public IActionResult PostItem(int menuId, [FromBody] MenuItemModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdItem = _menuService.AddItem(menuId, item);
                return Ok(createdItem);
            }
            catch (MenuNotFoundException)
            {
                return BadRequest();
            }
        }

        // PUT: api/Menus/5
        [HttpPut("items/{itemId}")]
        public IActionResult UpdateItem(int itemId, [FromBody] MenuItemModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                item.Id = itemId;
                _menuService.UpdateItem(item);
                return Ok(item);
            }
            catch (MenuItemNotFoundException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("items/{itemId}")]
        public IActionResult DeleteItem(int itemId)
        {
            try
            {
                _menuService.RemoveItem(itemId);
                return Ok();
            }
            catch (MenuItemNotFoundException)
            {
                return BadRequest();
            }
        }
    }
}
