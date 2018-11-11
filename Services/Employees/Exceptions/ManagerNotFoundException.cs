using Common;

namespace Services.Employees.Exceptions
{
    public class ManagerNotFoundException : CustomWebException
    {
        public override string Message => "Restaurant Manager not found.";
    }
}
