using System.Linq;
using AutoMapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Menus.Exceptions;
using Services.Menus.Models;
using Services.Restaurants.Exceptions;

namespace Services.Menus
{
    public class MenuService : IMenuService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IMapper _mapper;

        public MenuService(FoodFusionContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public MenuModel AddMenuIfNotExists(int restaurantId, MenuModel menuModel)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Menu)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();
            if(restaurant.Menu != null) throw new MenuAlreadyExistsException();

            var menu = _mapper.Map<Menu>(menuModel);
            restaurant.Menu = menu;
            
            _dbContext.SaveChanges();

            return _mapper.Map<MenuModel>(menu);
        }

        public MenuModel GetMenu(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Menu)
                    .ThenInclude(m => m.Items)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            var menu = restaurant.Menu ?? throw new MenuNotFoundException();

            return _mapper.Map<MenuModel>(menu);
        }

        public void RemoveMenu(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Menu)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();
            restaurant.Menu = restaurant.Menu ?? throw new MenuNotFoundException();
            
            _dbContext.Menus.Remove(restaurant.Menu);
            _dbContext.SaveChanges();
        }

        public MenuItemModel AddItem(int menuId, MenuItemModel menuItemModel)
        {
            var menu = _dbContext.Menus
                .FirstOrDefault(m => m.Id == menuId);
            menu = menu ?? throw new MenuNotFoundException();

            var menuItem = _mapper.Map<MenuItem>(menuItemModel);
            menuItem.MenuId = menu.Id;

            _dbContext.MenuItems.Add(menuItem);
            _dbContext.SaveChanges();
            
            return _mapper.Map<MenuItemModel>(menuItem);
        }

        public void RemoveItem(int id)
        {
            var menuItem = _dbContext.MenuItems
                .FirstOrDefault(m => m.Id == id);
            menuItem = menuItem ?? throw new MenuItemNotFoundException();

            _dbContext.MenuItems.Remove(menuItem);
            _dbContext.SaveChanges();
        }

        public void UpdateItem(MenuItemModel menuItemModel)
        {
            var menuItem = _dbContext.MenuItems
                   .FirstOrDefault(r => r.Id == menuItemModel.Id);
            menuItem = menuItem ?? throw new MenuItemNotFoundException();

            menuItem = _mapper.Map(menuItemModel, menuItem);

            _dbContext.SaveChanges();
        }
    }
}
