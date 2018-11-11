using Common;

namespace Services.Authentication.Exceptions
{
    public class UserNotFoundException : CustomWebException
    {
        public override string Message => "User not found.";
    }
}
