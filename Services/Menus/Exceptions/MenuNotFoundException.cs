using Common;

namespace Services.Menus.Exceptions
{
    public class MenuNotFoundException : CustomWebException
    {
        public override string Message => "Menu not found.";
    }
}
