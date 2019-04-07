using Common;

namespace Services.Hotel.Exceptions
{
    public class HotelRoomNotFoundException : CustomWebException
    {
        public override string Message => "Hotel Room not found.";
    }
}
