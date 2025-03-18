using EmployeeManagementSystemLoginHr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EmployeeManagementSystemLoginHr.Controllers
{
    /// <summary>
    /// Manages user-related operations, including retrieving users and fetching user details.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Retrieves all users or users assigned to a specific role.
        /// </summary>
        /// <param name="role">Optional role name to filter users by role.</param>
        /// <returns>A list of users.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all users or users in a specific role", Description = "Returns all users if no role is specified, or users assigned to a specific role if provided.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of users retrieved successfully.")]
        public async Task<IActionResult> GetUsers( string? role)
        {
           if(role is not null)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                return Ok(usersInRole);
            }

            var users = await _userManager.Users.ToListAsync();
           
            return Ok(users);    
           
        }
        /// <summary>
        /// Retrieves user details by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>User details.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieve user details by ID", Description = "Fetches a user's details based on their unique ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "User details retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return Ok(user);
        }
        



    }
}
