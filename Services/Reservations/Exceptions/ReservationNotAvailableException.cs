using Common;

namespace Services.Reservations.Exceptions
{
    public class ReservationNotAvailableException : CustomWebException
    {
        public override string Message => "The reservation could not be made because of unabailability.";
    }
}
