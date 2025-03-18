using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.Models;
using EmployeeManagementSystemLoginHr.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementSystemLoginHr.Services
{
    public class AuthService : IAuthService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(UnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IOptions<JwtSettings> jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            Console.WriteLine(_jwtSettings.SecretKey);
           
        }

        public async Task<string> CreateJwtAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)

            }
            .Union(userClaims)
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtSettings.ExpireMinutes),
                signingCredentials: signingCredentials
                );


            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            
            return token;


        }

        public async Task<AuthResult> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //If user Has no role add user role
                if (!_userManager.GetRolesAsync(user).Result.Any())
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                var token = await CreateJwtAsync(user);
                return new AuthResult { Success = true, Token = token, TokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.ExpireMinutes) };
            }
            else
            {
                return new AuthResult { Success = false, Errors = new[] { "Email or Password is incorrect" } };
            }
            
        }



        public async Task<AuthResult> RegisterAsync(RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                //Add role to user
                await _userManager.AddToRoleAsync(user, "User");
                return new AuthResult
                {
                    Success = true,
                    Token = await CreateJwtAsync(user),
                    Message = "Registration successful",
                    TokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.ExpireMinutes)
                };
            }
            else
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Registration failed",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
        }
           public async Task<AuthResult> ChangePasswordAsync(string userId, ChangePasswordModel model)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new AuthResult { Success= false, Message = "ApplicationUser not found." };
                }
                if(model.ConfirmPassword != model.NewPassword)
                {
                return new AuthResult { Success = false, Message = "Passwords do not match." };
                }
               

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return new AuthResult { Success = true, Message = "Password changed successfully." };
                }

                return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description) };
            }




        }
    }



