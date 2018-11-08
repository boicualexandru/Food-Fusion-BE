using System;
using Services.Menus.Models;

namespace Services.Menus
{
    public interface IMenuService
    {
        MenuModel AddMenuIfNotExists(int restaurantId, MenuModel menuModel);
        MenuModel GetMenu(int restaurantId);
        void RemoveMenu(int id);
        MenuItemModel AddItem(int menuId, MenuItemModel menuItemModel);
        void RemoveItem(int id);
    }
}
