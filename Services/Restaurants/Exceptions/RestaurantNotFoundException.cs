using Common;

namespace Services.Restaurants.Exceptions
{
    public class RestaurantNotFoundException : CustomWebException
    {
        public override string Message => "Restaurant not found.";
    }
}
