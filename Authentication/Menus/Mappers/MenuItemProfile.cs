using AutoMapper;
using DataAccess.Models;
using Services.Menus.Models;

namespace Services.Menus.Mappers
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, MenuItemModel>();
            CreateMap<MenuItemModel, MenuItem>();
        }
    }
}
