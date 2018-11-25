using Common;

namespace Services.Tables.Exceptions
{
    public class TableNotFoundException : CustomWebException
    {
        public override string Message => "Table(s) could not be found.";
    }
}
