using Common;

namespace Services.Menus.Exceptions
{
    public class MenuAlreadyExistsException : CustomWebException
    {
        public override string Message => "Menu already exists.";
    }
}
