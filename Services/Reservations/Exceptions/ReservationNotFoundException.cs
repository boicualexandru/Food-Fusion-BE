using Common;

namespace Services.Reservations.Exceptions
{
    public class ReservationNotFoundException : CustomWebException
    {
        public override string Message => "Reservation not found.";
    }
}
