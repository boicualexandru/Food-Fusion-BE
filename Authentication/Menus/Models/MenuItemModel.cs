using System.ComponentModel.DataAnnotations;

namespace Services.Menus.Models
{
    public class MenuItemModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public double Price { get; set; }
    }
}