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
    /// Manages role-based permissions for users and roles.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class PermissionController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public PermissionController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            
        }
        /// <summary>
        /// Retrieves all available permissions or permissions assigned to a specific role.
        /// </summary>
        /// <param name="roleName">Optional role name to get assigned permissions.</param>
        /// <returns>A list of permissions.</returns>
        [HttpGet]
        [Authorize(Policy = "ManagePermissions")]
        [SwaggerOperation(Summary = "Retrieve all or role-specific permissions", Description = "Returns a list of all permissions or only the permissions assigned to a specified role.")]
        [SwaggerResponse(200, "Successfully retrieved permissions", typeof(IEnumerable<string>))]
        [SwaggerResponse(404, "Role not found")]
        public async Task<IActionResult> GetPermissions([FromQuery] string? roleName)
        {
            if (roleName != null)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role is not null)
                {
                    var rolePermissions = await _roleManager.GetClaimsAsync(role);
                    return Ok(rolePermissions.Where(c => c.Type == Permissions.PermissionClaimType).Select(c => c.Value));
                }
                return NotFound(new
                {
                    error= "Role not found"
                });
            }
            var permissions = Permissions.GenerateAllPermissions();
            return Ok(permissions);
        }


        /// <summary>
        /// Adds a permission to a role.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <param name="permissionName">The permission to be added.</param>
        /// <returns>A success or failure message.</returns>
        [HttpPost("AddPermissionToRole")]
        [Authorize(Policy = "ManagePermissions")]
        [SwaggerOperation(Summary = "Add a permission to a role", Description = "Assigns a specific permission to a given role.")]
        [SwaggerResponse(200, "Permission added to role.")]
        [SwaggerResponse(400, "Invalid input parameters.")]
        [SwaggerResponse(404, "Role not found")]
        public async Task<IActionResult> AddPermissionToRole(string? roleName, string? permissionName)
        {
            if (roleName != null && permissionName != null)
            {
                var permissions = Permissions.GenerateAllPermissions();
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role is not null)
                {
                    if (permissions.Contains(permissionName))
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(Permissions.PermissionClaimType, permissionName));
                        return Ok("Permission added to role.");
                    }
                    return BadRequest("Invalid permission name.");
                }
                return NotFound("Role not found");
            }
            ModelState.AddModelError("", "Role name and permission name are required.");
            return BadRequest(ModelState);
        }

        //Add permission to User
        /// <summary>
        /// Adds a permission to a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="permissionName">The permission to be added.</param>
        /// <returns>A success or failure message.</returns>
        [HttpPost("AddPermissionToUser")]
        [Authorize(Policy = "ManagePermissions")]
        [SwaggerOperation(Summary = "Add a permission to a user", Description = "Assigns a specific permission to a user by their user ID.")]
        [SwaggerResponse(200, "Permission added to user.")]
        [SwaggerResponse(400, "Invalid input parameters.")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> AddPermissionToUser( string? userId,  string? permissionName)
        {
            if (userId != null && permissionName != null)
            {
                var permissions = Permissions.GenerateAllPermissions();
                var user = await _userManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    if (permissions.Contains(permissionName))
                    {
                        await _userManager.AddClaimAsync(user, new Claim(Permissions.PermissionClaimType, permissionName));
                        return Ok("Permission added to user.");
                    }
                    return BadRequest("Invalid permission name.");
                }
                return NotFound("User not found");
            }
            ModelState.AddModelError("", "User name and permission name are required.");
            return BadRequest(ModelState);
        }

        //Get User Permissions
        /// <summary>
        /// Retrieves all permissions assigned to a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A list of permissions.</returns>
        [HttpGet("UserPermissions")]
        [Authorize(Policy = "ManagePermissions")]
        [SwaggerOperation(Summary = "Retrieve user-specific permissions", Description = "Returns a list of permissions assigned to a specified user.")]
        [SwaggerResponse(200, "Successfully retrieved user permissions", typeof(IEnumerable<string>))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetUserPermissions([FromQuery] string? userId)
        {
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    var userPermissions = userClaims.Where(c => c.Type == Permissions.PermissionClaimType).Select(c => c.Value).ToList();
                    return Ok(userPermissions);
                }
                return NotFound("User not found");
            }
            ModelState.AddModelError("", "User name is required.");
            return BadRequest(ModelState);

        }

        //remove permission from role
        [HttpDelete("RemovePermissionFromRole")]
        [Authorize(Policy = "ManagePermissions")]
        public async Task<IActionResult> RemovePermissionFromRole(string? roleName, string? permissionName)
        {
            if (roleName != null && permissionName != null)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role is not null)
                {
                    await _roleManager.RemoveClaimAsync(role, new Claim(Permissions.PermissionClaimType, permissionName));
                    return Ok("Permission removed from role.");
                }
                return NotFound("Role not found");
            }
            ModelState.AddModelError("", "Role name and permission name are required.");
            return BadRequest(ModelState);
        }

        //remove permission from user
        [HttpDelete("RemovePermissionFromUser")]
        [Authorize(Policy = "ManagePermissions")]
        public async Task<IActionResult> RemovePermissionFromUser(string? userId, string? permissionName)
        {
            if (userId != null && permissionName != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    await _userManager.RemoveClaimAsync(user, new Claim(Permissions.PermissionClaimType, permissionName));
                    return Ok("Permission removed from user.");
                }
                return NotFound("User not found");
            }
            ModelState.AddModelError("", "User name and permission name are required.");
            return BadRequest(ModelState);
        }


    }
}
