using Common;

namespace Services.Hotel.Exceptions
{
    public class HotelBookingInvalidDetailesException : CustomWebException
    {
        public override string Message => "Booking details are invalid.";
    }
}
