using Common;

namespace Services.Reservations.Exceptions
{
    public class TablesAreNotFittingException : CustomWebException
    {
        public override string Message => "Tables are not fitting the number of participants.";
    }
}
