using Common;

namespace Services.Menus.Exceptions
{
    public class MenuItemNotFoundException : CustomWebException
    {
        public override string Message => "Menu Item not found.";
    }
}
