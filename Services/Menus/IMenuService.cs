using System;
using Services.Menus.Models;

namespace Services.Menus
{
    public interface IMenuService
    {
        MenuModel AddMenuIfNotExists(int restaurantId, MenuModel menuModel);
        MenuModel GetMenu(int restaurantId);
        void RemoveMenu(int restaurantId);
        MenuItemModel AddItem(int menuId, MenuItemModel menuItem);
        void UpdateItem(MenuItemModel menuItem);
        void RemoveItem(int id);
    }
}
