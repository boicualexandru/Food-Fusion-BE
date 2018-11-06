using AutoMapper;
using DataAccess.Models;
using Services.Menus.Models;

namespace Services.Menus.Mappers
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuModel>();
            CreateMap<MenuModel, Menu>()
                .ForMember(menu => menu.Id, opt => opt.Ignore());
        }
    }
}
