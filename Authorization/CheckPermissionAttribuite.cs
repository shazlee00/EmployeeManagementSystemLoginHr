using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagementSystemLoginHr.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckPermissionAttribute:Attribute
    {
        public CheckPermissionAttribute(string permissionName)
        {
            PermissionName = permissionName;
        }
        public string PermissionName { get; set; }
    }



}
