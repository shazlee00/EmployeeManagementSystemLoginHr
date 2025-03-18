using EmployeeManagementSystemLoginHr.Helpers;
using EmployeeManagementSystemLoginHr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EmployeeManagementSystemLoginHr.Authorization
{
    public class PermissionsBasedAuthorizationFilter : IAuthorizationFilter
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionsBasedAuthorizationFilter(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
       

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attribute = context.ActionDescriptor.EndpointMetadata.OfType<CheckPermissionAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                var _permissionName = attribute.PermissionName;
                if (!Permissions.GenerateAllPermissions().Contains(_permissionName))
                {
                    context.Result = new BadRequestResult();
                }
                else
                {
                    var claimIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                    if(claimIdentity is null || !claimIdentity.IsAuthenticated)
                    {
                        context.Result = new ForbidResult();
                    }

                    var userIdClaim = claimIdentity.Claims.FirstOrDefault(c => c.Type == "uid");
                    if (userIdClaim is null)
                    {
                        context.Result = new ForbidResult();
                    }
                    var userId = userIdClaim.Value;
                    
                    var userPermissions = claimIdentity.Claims.Where(c => c.Type == Permissions.PermissionClaimType).Select(c => c.Value);
                    var userRoles= _userManager.GetRolesAsync(_userManager.FindByIdAsync(userId).Result).Result.ToList();

                    foreach (var role in userRoles)
                    {
                        var roleClaims = _roleManager.GetClaimsAsync(_roleManager.FindByNameAsync(role).Result).Result;
                        var rolePermissions = roleClaims.Where(c => c.Type == Permissions.PermissionClaimType).Select(c => c.Value);
                        userPermissions = userPermissions.Union(rolePermissions);
                    }
                    
                    
                    if (!userPermissions.Contains(_permissionName))
                    {
                        context.Result = new ForbidResult();
                    }



                  
                }
            }
        }
    }



}
