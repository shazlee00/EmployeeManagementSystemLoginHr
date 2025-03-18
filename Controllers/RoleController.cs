using EmployeeManagementSystemLoginHr.Helpers;
using EmployeeManagementSystemLoginHr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace EmployeeManagementSystemLoginHr.Controllers
{
    /// <summary>
    /// Manages role-based operations such as role creation, assignment, and retrieval.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]

    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves all available roles.
        /// </summary>
        /// <returns>A list of roles.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all roles", Description = "Returns a list of all available roles.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of roles retrieved successfully.")]
        public async Task<IActionResult> GetRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }




        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="roleName">The name of the role to add.</param>
        /// <returns>A success or failure message.</returns>
        [HttpPost]
        [Authorize(Policy = "ManagePermissions")]
        [SwaggerOperation(Summary = "Add a new role", Description = "Creates a new role if it does not already exist.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Role created successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Role already exists or role name is required.")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (_roleManager.Roles.Any(r => r.Name == roleName))
                return BadRequest(new
                {
                    error = "Role already exists"
                });
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
                return Ok();
            }

            return BadRequest(new
            {
                error= "Role name is required"
            });
        }
        /// <summary>
        /// Assigns a user to a role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A success or failure message.</returns>
        [HttpPost("AssignUserToRole")]
        [Authorize(Policy = "AssignUserRole")]
        [SwaggerOperation(Summary = "Assign a user to a role", Description = "Assigns a specified user to a specified role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "User assigned to role successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User or role not found.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error occurred while assigning user to role.")]
        public async Task<IActionResult> AssignUserToRole(string roleName, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return NotFound("Role not found");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded) return Ok("User assigned to role.");
            return BadRequest(result.Errors);
        }



        /// <summary>
        /// Retrieves all roles assigned to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of roles assigned to the user.</returns>
        [HttpGet("UserRoles")]
        [Authorize(Policy = "AssignUserRole")]
        [SwaggerOperation(Summary = "Retrieve user roles", Description = "Returns all roles assigned to a specified user.")]
        [SwaggerResponse(StatusCodes.Status200OK, "User roles retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "User ID is required.")]
        public async Task<IActionResult> GetUserRoles([FromQuery]string? userId)
        {
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    return Ok(userRoles);
                }
                return NotFound("User not found");
            }
            return BadRequest("User name is required.");
        }

        //Remove Role From User
        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roleName">The role to remove.</param>
        /// <returns>A success or failure message.</returns>
        [HttpDelete("RemoveRoleFromUser")]
        [Authorize(Policy = "AssignUserRole")]
        [SwaggerOperation(Summary = "Remove a role from a user", Description = "Removes a specified role from a specified user.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Role removed from user successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "User ID and role name are required.")]
        public async Task<IActionResult> RemoveRoleFromUser(string? userId, string? roleName)
        {
            if (userId != null && roleName != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                    return Ok("Role removed from user.");
                }
                return NotFound("User not found");
            }
            return BadRequest("User name and role name are required.");
        }

     




    }
}
