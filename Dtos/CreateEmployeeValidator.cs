using EmployeeManagementSystemLoginHr.UOW;
using FluentValidation;

namespace EmployeeManagementSystemLoginHr.Dtos
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        private readonly UnitOfWork _unitOfWork;

        public CreateEmployeeValidator(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(e => e.EmployeeCode)
                .NotEmpty().WithMessage("Employee Code is required")
                .Length(1, 50).WithMessage("Employee Code must be between 1 and 50 characters")
                .MustAsync(BeUniqueEmployeeCode).WithMessage("Employee Code already exists");

            RuleFor(e => e.NationalId)
                .NotEmpty().WithMessage("National ID is required")
                .Matches(@"^\d{14}$").WithMessage("National ID must be 14 digits")
                .MustAsync(BeUniqueNationalId).WithMessage("National ID already exists");

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .Length(1, 100).WithMessage("Email must be between 1 and 100 characters")
                .MustAsync(BeUniqueEmail).WithMessage("Email already exists");

            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .Length(1, 50).WithMessage("First Name must be between 1 and 50 characters");

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .Length(1, 50).WithMessage("Last Name must be between 1 and 50 characters");

            RuleFor(e => e.FirstNameAr)
                .NotEmpty().WithMessage("First Name (Arabic) is required")
                .Length(1, 50).WithMessage("First Name (Arabic) must be between 1 and 50 characters");

            RuleFor(e => e.LastNameAr)
                .NotEmpty().WithMessage("Last Name (Arabic) is required")
                .Length(1, 50).WithMessage("Last Name (Arabic) must be between 1 and 50 characters");

            RuleFor(e => e.PhoneNumber)
                .Matches(@"^\d{11,20}$").WithMessage("Phone Number must be between 11 and 20 characters");

            RuleFor(e => e.JobTitle)
                .NotEmpty().WithMessage("Job Title is required")
                .Length(1, 100).WithMessage("Job Title must be between 1 and 100 characters");

            RuleFor(e => e.JobTitleAr)
                .NotEmpty().WithMessage("Job Title (Arabic) is required")
                .Length(1, 100).WithMessage("Job Title (Arabic) must be between 1 and 100 characters");

            RuleFor(e => e.Department)
                .Length(0, 100).WithMessage("Department must be between 0 and 100 characters");

            RuleFor(e => e.DepartmentAr)
                .Length(0, 100).WithMessage("Department (Arabic) must be between 0 and 100 characters");

            RuleFor(e => e.Salary)
                .InclusiveBetween(0, 1000000).WithMessage("Salary must be between 0 and 1,000,000");
        }

        private async Task<bool> BeUniqueEmployeeCode(string employeeCode, CancellationToken cancellationToken)
        {
            return !await _unitOfWork.EmployeeRepository.AnyAsync(e => e.EmployeeCode == employeeCode, cancellationToken);
        }

        private async Task<bool> BeUniqueNationalId(string nationalId, CancellationToken cancellationToken)
        {
            return !await _unitOfWork.EmployeeRepository.AnyAsync(e => e.NationalId == nationalId, cancellationToken);
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _unitOfWork.EmployeeRepository.AnyAsync(e => e.Email == email, cancellationToken);
        }
    }



}
