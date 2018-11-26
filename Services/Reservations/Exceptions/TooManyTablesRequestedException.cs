using Common;

namespace Services.Reservations.Exceptions
{
    public class TooManyTablesRequestedException : CustomWebException
    {
        public override string Message => "Too many tables requested.";
    }
}
