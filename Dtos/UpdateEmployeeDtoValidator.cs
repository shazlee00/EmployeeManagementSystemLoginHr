using EmployeeManagementSystemLoginHr.UOW;
using FluentValidation;

namespace EmployeeManagementSystemLoginHr.Dtos
{
    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {
        private readonly UnitOfWork _unitOfWork;

        public UpdateEmployeeDtoValidator(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Matches(@"^\d{14}$").WithMessage("National ID must be 14 digits.")
                .MustAsync(BeUniqueNationalId).WithMessage("National ID must be unique.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .Length(1, 50).WithMessage("First Name must be between 1 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Length(1, 50).WithMessage("Last Name must be between 1 and 50 characters.");

            RuleFor(x => x.FirstNameAr)
                .NotEmpty().WithMessage("First Name (Arabic) is required.")
                .Length(1, 50).WithMessage("First Name (Arabic) must be between 1 and 50 characters.");

            RuleFor(x => x.LastNameAr)
                .NotEmpty().WithMessage("Last Name (Arabic) is required.")
                .Length(1, 50).WithMessage("Last Name (Arabic) must be between 1 and 50 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Length(11, 20).WithMessage("Phone Number must be between 11 and 20 characters.");

            RuleFor(x => x.JobTitle)
                .NotEmpty().WithMessage("Job Title is required.")
                .Length(1, 100).WithMessage("Job Title must be between 1 and 100 characters.");

            RuleFor(x => x.JobTitleAr)
                .NotEmpty().WithMessage("Job Title (Arabic) is required.")
                .Length(1, 100).WithMessage("Job Title (Arabic) must be between 1 and 100 characters.");

            RuleFor(x => x.Department)
                .Length(0, 100).WithMessage("Department must be between 0 and 100 characters.");

            RuleFor(x => x.DepartmentAr)
                .Length(0, 100).WithMessage("Department (Arabic) must be between 0 and 100 characters.");

            RuleFor(x => x.Salary)
                .InclusiveBetween(0, 1000000).WithMessage("Salary must be between 0 and 1,000,000.");
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
