using Common;

namespace Services.Hotel.Exceptions
{
    public class HotelRoomGuestsExceededException : CustomWebException
    {
        public override string Message => "Guests number exceeded room capacity.";
    }
}
