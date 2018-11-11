using Common;

namespace Services.Employees.Exceptions
{
    public class EmployeeAlreadyExistsException : CustomWebException
    {
        public override string Message => "Employee already exists.";
    }
}
