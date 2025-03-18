using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.Models;

namespace EmployeeManagementSystemLoginHr.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterModel model);
        Task<AuthResult> LoginAsync(LoginModel model);
        Task<string> CreateJwtAsync(ApplicationUser user);

        Task<AuthResult> ChangePasswordAsync(string userId, ChangePasswordModel model);

       


    }






}
