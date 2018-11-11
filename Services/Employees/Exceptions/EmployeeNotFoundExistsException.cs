using Common;

namespace Services.Employees.Exceptions
{
    public class EmployeeNotFoundException : CustomWebException
    {
        public override string Message => "Employee not found.";
    }
}
