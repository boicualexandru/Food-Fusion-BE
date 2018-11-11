using AutoMapper;
using DataAccess.Models;
using Services.Employees.Models;

namespace Services.Employees.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<User, EmployeeModel>();
        }
    }
}
