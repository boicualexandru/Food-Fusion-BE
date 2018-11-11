using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Authorization;
using Services.Menus;
using Services.Menus.Exceptions;
using Services.Menus.Models;
using Services.Restaurants;

namespace WebApi.Controllers
{
    [Route("api/Restaurants")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IResourceAuthorizationService<RestaurantAuthorizationRequirement> _restaurantAuthorizationService;
        private readonly IResourceAuthorizationService<MenuAuthorizationRequirement> _menuAuthorizationService;
        private readonly IResourceAuthorizationService<MenuItemAuthorizationRequirement> _menuItemAuthorizationService;
        private readonly IMenuService _menuService;

        public MenuController(
            IResourceAuthorizationService<RestaurantAuthorizationRequirement> restaurantAuthorizationService, 
            IResourceAuthorizationService<MenuAuthorizationRequirement> menuAuthorizationService, 
            IResourceAuthorizationService<MenuItemAuthorizationRequirement> menuItemAuthorizationService, 
            IMenuService menuService)
        {
            _restaurantAuthorizationService = restaurantAuthorizationService;
            _menuAuthorizationService = menuAuthorizationService;
            _menuItemAuthorizationService = menuItemAuthorizationService;
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
            catch (MenuNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Restaurants/5/Menu
        [Authorize]
        [HttpPost("{restaurantId}/[controller]")]
        public IActionResult Post(int restaurantId, [FromBody] MenuModel menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isAuthorized = _restaurantAuthorizationService
                .WithUser(User)
                .WithRequirement(Operations<RestaurantAuthorizationRequirement>.Update)
                .WithResource(restaurantId)
                .IsAuthorized();
            if (!isAuthorized) return Forbid();

            var createdMenu = _menuService.AddMenuIfNotExists(restaurantId, menu);
            return Ok(createdMenu);
        }

        // DELETE: api/Restaurants/5/Menu
        [Authorize]
        [HttpDelete("{restaurantId}/[controller]")]
        public IActionResult Delete(int restaurantId)
        {
            try
            {
                var isAuthorized = _restaurantAuthorizationService
                   .WithUser(User)
                   .WithRequirement(Operations<RestaurantAuthorizationRequirement>.Update)
                   .WithResource(restaurantId)
                   .IsAuthorized();
                if (!isAuthorized) return Forbid();

                _menuService.RemoveMenu(restaurantId);
                return Ok();
            }
            catch (MenuNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Restaurants/Menu/5/Items
        [Authorize]
        [HttpPost("[controller]/{menuId}/Items")]
        public IActionResult PostItem(int menuId, [FromBody] MenuItemModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isAuthorized = _menuAuthorizationService
                   .WithUser(User)
                   .WithRequirement(Operations<MenuAuthorizationRequirement>.Update)
                   .WithResource(menuId)
                   .IsAuthorized();
                if (!isAuthorized) return Forbid();

                var createdItem = _menuService.AddItem(menuId, item);
                return Ok(createdItem);
            }
            catch (MenuNotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/Restaurants/Menu/Items/5
        [Authorize]
        [HttpPut("[controller]/Items/{itemId}")]
        public IActionResult UpdateItem(int itemId, [FromBody] MenuItemModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isAuthorized = _menuItemAuthorizationService
                      .WithUser(User)
                      .WithRequirement(Operations<MenuItemAuthorizationRequirement>.Update)
                      .WithResource(itemId)
                      .IsAuthorized();
                if (!isAuthorized) return Forbid();

                item.Id = itemId;
                _menuService.UpdateItem(item);
                return Ok(item);
            }
            catch (MenuItemNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Restaurants/Menu/Items/5
        [Authorize]
        [HttpDelete("[controller]/Items/{itemId}")]
        public IActionResult DeleteItem(int itemId)
        {
            try
            {
                var isAuthorized = _menuItemAuthorizationService
                      .WithUser(User)
                      .WithRequirement(Operations<MenuItemAuthorizationRequirement>.Delete)
                      .WithResource(itemId)
                      .IsAuthorized();
                if (!isAuthorized) return Forbid();

                _menuService.RemoveItem(itemId);
                return Ok();
            }
            catch (MenuItemNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
