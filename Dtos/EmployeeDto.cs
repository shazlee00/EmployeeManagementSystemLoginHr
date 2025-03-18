using EmployeeManagementSystemLoginHr.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemLoginHr.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public required string EmployeeCode { get; set; }
        public required string NationalId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string FirstNameAr { get; set; }
        public required string LastNameAr { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string JobTitle { get; set; }
        public required string JobTitleAr { get; set; }
        public string? Department { get; set; }
        public string? DepartmentAr { get; set; }

        public decimal Salary { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active; // Default to Active Status



        public string? EmployeeFullNameEn;

        public string? EmployeeFullNameAr;


    }


}
