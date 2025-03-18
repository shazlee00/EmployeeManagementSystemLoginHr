using AutoMapper;
using EmployeeManagementSystemLoginHr.Models;

namespace EmployeeManagementSystemLoginHr.Dtos
{
    public class EmployeeMappingProfile:Profile
    {
        public EmployeeMappingProfile() {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<CreateEmployeeDto, Employee>().ReverseMap();
            CreateMap<UpdateEmployeeDto, Employee>().ReverseMap();
        }
    }
}
