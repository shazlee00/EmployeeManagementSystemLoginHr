using EmployeeManagementSystemLoginHr.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemLoginHr.Dtos
{
    public class CreateEmployeeDto
    {
        [Required, StringLength(50, ErrorMessage = "Employee Code must be between 1 and 50 characters")]
        public string EmployeeCode { get; set; }

        [Required, RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be 14 digits")]
        public string NationalId { get; set; }

        [Required, StringLength(50, ErrorMessage = "First Name must be between 1 and 50 characters")]
        public string FirstName { get; set; }

        [Required, StringLength(50, ErrorMessage = "Last Name must be between 1 and 50 characters")]
        public string LastName { get; set; }

        [Required, StringLength(50, ErrorMessage = "First Name (Arabic) must be between 1 and 50 characters")]
        public string FirstNameAr { get; set; }

        [Required, StringLength(50, ErrorMessage = "Last Name (Arabic) must be between 1 and 50 characters")]
        public string LastNameAr { get; set; }

        [Required, EmailAddress, StringLength(100, ErrorMessage = "Email must be between 1 and 100 characters")]
        public string Email { get; set; }

        [Phone, StringLength(20,MinimumLength =11, ErrorMessage = "Phone Number must be between 11 and 20 characters")]
        public string? PhoneNumber { get; set; }

        [Required, StringLength(100, ErrorMessage = "Job Title must be between 1 and 100 characters")]
        public string JobTitle { get; set; }

        [Required, StringLength(100, ErrorMessage = "Job Title (Arabic) must be between 1 and 100 characters")]
        public string JobTitleAr { get; set; }

        [StringLength(100, ErrorMessage = "Department must be between 0 and 100 characters")]
        public string? Department { get; set; }

        [StringLength(100, ErrorMessage = "Department (Arabic) must be between 0 and 100 characters")]
        public string? DepartmentAr { get; set; }

        [Range(0, 1000000, ErrorMessage = "Salary must be between 0 and 1,000,000")]
        public decimal Salary { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active; // Default to Active Status
    }




}
