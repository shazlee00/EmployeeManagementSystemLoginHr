using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemLoginHr.Dtos
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]

        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required, EmailAddress]

        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}