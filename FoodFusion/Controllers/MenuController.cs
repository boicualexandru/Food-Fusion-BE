﻿using Microsoft.AspNetCore.Mvc;
using Services.Menus;
using Services.Menus.Exceptions;
using Services.Menus.Models;
using WebApi.ActionFilters;

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
            catch (MenuNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Restaurants/5/Menu
        [AuthorizeByRestaurant(roles: "Manager")]
        [HttpPost("{restaurantId}/[controller]")]
        public IActionResult Post(int restaurantId, [FromBody] MenuModel menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMenu = _menuService.AddMenuIfNotExists(restaurantId, menu);
            return Ok(createdMenu);
        }

        // DELETE: api/Restaurants/5/Menu
        [AuthorizeByRestaurant(roles: "Manager")]
        [HttpDelete("{restaurantId}/[controller]")]
        public IActionResult Delete(int restaurantId)
        {
            try
            {
                _menuService.RemoveMenu(restaurantId);
                return Ok();
            }
            catch (MenuNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Restaurants/Menu/5/Items
        [AuthorizeByMenu(roles: "Manager")]
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
                return NotFound();
            }
        }

        // PUT: api/Restaurants/Menu/Items/5
        [AuthorizeByMenuItem(roles: "Manager")]
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
                return NotFound();
            }
        }

        // DELETE: api/Restaurants/Menu/Items/5
        [AuthorizeByMenuItem(roles: "Manager")]
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
                return NotFound();
            }
        }
    }
}
