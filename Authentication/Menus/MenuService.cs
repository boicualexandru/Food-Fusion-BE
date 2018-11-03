using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DataAccess.Models;
using Services.Menus.Models;

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

        public MenuModel ReplaceMenu(int restaurantId, MenuModel menuModel)
        {
            throw new NotImplementedException();
        }

        public MenuModel GetMenu(int restaurantId)
        {
            throw new NotImplementedException();
        }

        public MenuItemModel AddItem(int restaurantId, MenuItemModel menuItemModel)
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(int menuItemId)
        {
            throw new NotImplementedException();
        }
    }
}
