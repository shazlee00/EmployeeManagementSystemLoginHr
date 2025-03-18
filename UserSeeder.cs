using EmployeeManagementSystemLoginHr.Enums;
using EmployeeManagementSystemLoginHr.Helpers;
using EmployeeManagementSystemLoginHr.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace EmployeeManagementSystemLoginHr
{
    public static class UserSeeder
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                FirstName = "User",
                LastName = "User",
                UserName = "Useruser@domain.com",
                Email = "Useruser@domain.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRoleAsync(defaultUser, Role.User.ToString());
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger)
        {
            var defaultUser = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "Admin@domain.com",
                Email = "Admin@domain.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRolesAsync(defaultUser, [Role.Admin.ToString(), Role.User.ToString()]);
                await userManager.AddClaimsAsync(user,
                [
              new Claim(Permissions.PermissionClaimType,Permissions.AssignRolePermission),
                new Claim(Permissions.PermissionClaimType,Permissions.ManagePermissionsPermission),

                ]);
            }
            await roleManger.SeedClaimsForSuperUser();
          
        }

        private static async Task SeedClaimsForSuperUser(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Role.Admin.ToString());
            await roleManager.AddPermissionClaims(adminRole);
        }

        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GenerateAllModulesPermissions();

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
