using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.Helpers;
using EmployeeManagementSystemLoginHr.ResourceParameters;

namespace EmployeeManagementSystemLoginHr.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);

        Task<EmployeeDto> GetEmployeeAsync(int id);
        Task<bool> CheckEmployeeCodeExistsAsync(string employeeCode);
        Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
        Task<PagedList<EmployeeDto>> GetEmployeesAsync( EmployeeParameters employeeParameters);
        Task DeleteEmployeeAsync(int id);
    }
}
