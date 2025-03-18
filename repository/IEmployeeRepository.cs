using EmployeeManagementSystemLoginHr.Helpers;
using EmployeeManagementSystemLoginHr.Models;
using EmployeeManagementSystemLoginHr.ResourceParameters;

namespace EmployeeManagementSystemLoginHr.repository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<bool> EmployeeCodeExistsAsync(string employeeCode);
        Task<PagedList<Employee>> GetFilteredEmployees(EmployeeParameters employeeParameters);
    }
}
