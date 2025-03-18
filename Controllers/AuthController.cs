using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.Models;
using EmployeeManagementSystemLoginHr.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace EmployeeManagementSystemLoginHr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">The registration model containing user details.</param>
        /// <returns>Returns success or failure response.</returns>
        /// <response code="200">User registered successfully.</response>
        /// <response code="400">Invalid request or user already exists.</response>
        [SwaggerOperation(Summary = "Registers a new user", Description = "Creates a new account with the provided details.")]
        [SwaggerResponse(200, "User registered successfully", typeof(AuthResult))]
        [SwaggerResponse(400, "Invalid input or user already exists")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var authResult = await _authService.RegisterAsync(model);
            if(authResult.Success)
            {
                return Ok(authResult);
            }
            return BadRequest(authResult);
            
        }


        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="model">The login model containing username and password.</param>
        /// <returns>Returns a JWT token if successful.</returns>
        /// <response code="200">Login successful.</response>
        /// <response code="400">Invalid credentials.</response>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Logs in a user", Description = "Authenticates the user and returns a JWT token.")]
        [SwaggerResponse(200, "Login successful", typeof(AuthResult))]
        [SwaggerResponse(400, "Invalid username or password")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result= await _authService.LoginAsync(model);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        /// <summary>
        /// Changes the password of the authenticated user.
        /// </summary>
        /// <param name="model">The password change model.</param>
        /// <returns>Returns success or failure response.</returns>
        /// <response code="200">Password changed successfully.</response>
        /// <response code="400">Invalid request.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="500">Unexpected error.</response>
        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerOperation(Summary = "Changes user password", Description = "Allows an authenticated user to change their password.")]
        [SwaggerResponse(200, "Password changed successfully", typeof(AuthResult))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(401, "User is not authenticated")]
        [SwaggerResponse(500, "Unexpected error")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {

            var userName=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user=await _userManager.FindByNameAsync(userName);
            var userId=user.Id;


           

            if (userId == null)
            {
                return Problem(
                title: "An unexpected error occurred.",
                statusCode: 500
      
                );
            }
            var result = await _authService.ChangePasswordAsync(userId,model);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }




    }
}
