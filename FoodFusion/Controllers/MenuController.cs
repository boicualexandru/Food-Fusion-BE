using Microsoft.AspNetCore.Mvc;
using Services.Menus;
using Services.Menus.Exceptions;
using Services.Menus.Models;
using Services.Restaurants.Exceptions;

namespace WebApi.Controllers
{
    [Route("api/Restaurants")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/Restaurants/5/Menu
        [HttpGet("{restaurantId}/[controller]")]
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

        // POST: api/Restaurants/5/Menu
        [HttpPost("{restaurantId}/[controller]")]
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

        // DELETE: api/Restaurants/5/Menu
        [HttpDelete("{restaurantId}/[controller]")]
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

        // POST: api/Restaurants/Menu/5/Items
        [HttpPost("[controller]/{menuId}/Items")]
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

        // PUT: api/Restaurants/Menu/Items/5
        [HttpPut("[controller]/Items/{itemId}")]
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

        // DELETE: api/Restaurants/Menu/Items/5
        [HttpDelete("[controller]/Items/{itemId}")]
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
