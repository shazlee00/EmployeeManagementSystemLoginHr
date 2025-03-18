using EmployeeManagementSystemLoginHr.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemLoginHr.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string EmployeeCode { get; set; }

        [Required, StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be exactly 14 characters.")]
        public required string NationalId { get; set; }

        [Required, MaxLength(50)]
        public required string FirstName { get; set; }

        [Required, MaxLength(50)]
        public required string LastName { get; set; }

        [Required, MaxLength(50)]
        public required string FirstNameAr { get; set; }

        [Required, MaxLength(50)]
        public required string LastNameAr { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public required string Email { get; set; }

        [Phone, MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required, MaxLength(100)]
        public required string JobTitle { get; set; }

        [Required, MaxLength(100)]
        public required string JobTitleAr { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        [MaxLength(100)]
        public string? DepartmentAr { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [NotMapped]
        public string EmployeeFullNameEn => $"{FirstName} {LastName}";

        [NotMapped]
        public string EmployeeFullNameAr => $"{FirstNameAr ?? ""} {LastNameAr ?? ""}".Trim();

        [Required]
        public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active;
    }


}
