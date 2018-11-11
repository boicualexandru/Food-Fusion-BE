using Common;

namespace Services.Restaurants.Exceptions
{
    public class ManagerNotFoundException : CustomWebException
    {
        public override string Message => "Restaurant Manager not found.";
    }
}
