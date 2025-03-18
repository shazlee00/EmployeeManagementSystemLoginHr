using EmployeeManagementSystemLoginHr.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemLoginHr.Models
{
    public class ApplicationUser:IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    
    }
}