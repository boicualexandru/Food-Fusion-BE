﻿using Services.Menus.Models;

namespace Services.Menus
{
    interface IMenuService
    {
        MenuModel AddMenuIfNotExists(MenuModel menuModel);
        MenuModel GetMenu(int restaurantId);
        MenuItemModel AddItem(int restaurantId, MenuItemModel menuItemModel);
        void RemoveItem(int menuItemId);
    }
}
