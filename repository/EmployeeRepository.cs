using EmployeeManagementSystemLoginHr.Context;
using EmployeeManagementSystemLoginHr.Helpers;
using EmployeeManagementSystemLoginHr.Models;
using EmployeeManagementSystemLoginHr.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemLoginHr.repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<bool> EmployeeCodeExistsAsync(string employeeCode)
        {
            return _context.Employees.AnyAsync(e => e.EmployeeCode == employeeCode);
        }
        public async Task<PagedList<Employee>> GetFilteredEmployees(EmployeeParameters employeeParameters)
        {
            var employees = _context.Employees
                .Where(e => string.IsNullOrEmpty(employeeParameters.Name) || e.FirstName.Contains(employeeParameters.Name) || e.LastName.Contains(employeeParameters.Name))
                .Where(e => string.IsNullOrEmpty(employeeParameters.JobTitle) || e.JobTitle == employeeParameters.JobTitle)
                .Where(e => employeeParameters.MinSalary == null || e.Salary >= employeeParameters.MinSalary)
                .Where(e => employeeParameters.MaxSalary == null || e.Salary <= employeeParameters.MaxSalary);
             
                return await PagedList<Employee>.ToPagedListAsync(employees, employeeParameters.PageNumber, employeeParameters.PageSize);
        }
    }
}
