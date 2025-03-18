using EmployeeManagementSystemLoginHr.Enums;
using System.Reflection;

namespace EmployeeManagementSystemLoginHr.Helpers
{

    public class Permissions
    {
        public const string AssignRolePermission = "Permissions.Role.AssignRoles";
        public const string PermissionClaimType = "Permission";
        public const string ManagePermissionsPermission = "Permissions.Role.ManagePermissions";

        public static List<string> GeneratePermissionsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }

        public static List<string> GenerateAllModulesPermissions()
        {
            var allPermissions = new List<string>();

            var modules = Enum.GetValues(typeof(Modules));

            foreach (var module in modules)
                allPermissions.AddRange(GeneratePermissionsList(module.ToString()));

            return allPermissions;
        }

        public static List<string> GenerateAllPermissions()
        {
            var allPermissions = new List<string>
            {
                AssignRolePermission,
                ManagePermissionsPermission
            }
            .Concat(GenerateAllModulesPermissions()).ToList();
            return allPermissions;
        }

        public class EmployeePermissions
        {
            public const string View = "Permissions.Employee.View";
            public const string Create = "Permissions.Employee.Create";
            public const string Update = "Permissions.Employee.Edit";
            public const string Delete = "Permissions.Employee.Delete";

        }


    }


}
