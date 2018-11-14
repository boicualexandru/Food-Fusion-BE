using Services.Employees.Models;
using System.Collections.Generic;

namespace Services.Employees
{
    public interface IEmployeesService
    {
        IList<EmployeeModel> GetEmployees(int restaurantId);
        EmployeeModel AddEmployee(int restaurantId, int userId);
        void RemoveEmployee(int restaurantId, int userId);

        EmployeeModel GetManager(int restaurantId);
        EmployeeModel AddOrReplaceManager(int restaurantId, int userId);
    }
}
