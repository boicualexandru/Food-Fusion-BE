using Common;

namespace Services.Hotel.Exceptions
{
    public class HotelRoomUnavailableException : CustomWebException
    {
        public override string Message => "Hotel Room is unavailable for the selected period.";
    }
}
