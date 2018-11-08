using System.Collections.Generic;

namespace Services.Menus.Models
{
    public class MenuModel
    {
        public int Id { get; set; }

        public List<MenuItemModel> Items { get; set; }
    }
}