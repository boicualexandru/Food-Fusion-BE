using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Menus.Exceptions;
using Services.Menus.Models;
using Services.Restaurants.Exceptions;

namespace Services.Menus
{
    class MenuService : IMenuService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IMapper _mapper;

        public MenuService(FoodFusionContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public MenuModel AddMenuIfNotExists(MenuModel menuModel)
        {
            var menu = _mapper.Map<Menu>(menuModel);

            try
            {
                // TODO: replace restaurant checking with FK violation exception
                var restaurant = _dbContext.Restaurants
                    .FirstOrDefault(r => r.Id == menuModel.RestaurantId);
                restaurant = restaurant ?? throw new RestaurantNotFoundException();

                _dbContext.Menus.Add(menu);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return _mapper.Map<MenuModel>(menu);
        }

        public MenuModel GetMenu(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Menu)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            var menu = restaurant.Menu ?? throw new MenuNotFoundException();

            return _mapper.Map<MenuModel>(menu);
        }

        public MenuItemModel AddItem(int restaurantId, MenuItemModel menuItemModel)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Menu)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            var menu = restaurant.Menu ?? throw new MenuNotFoundException();

            var menuItem = _mapper.Map<MenuItem>(menuItemModel);
            menu.Items.Add(menuItem);

            _dbContext.SaveChanges();

            return _mapper.Map<MenuItemModel>(menuItem);
        }

        public void RemoveItem(int menuItemId)
        {
            var menuItem = new MenuItem { Id = menuItemId };
            _dbContext.MenuItems.Attach(menuItem);
            _dbContext.MenuItems.Remove(menuItem);

            _dbContext.SaveChanges();
        }
    }
}
