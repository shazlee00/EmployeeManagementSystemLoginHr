using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeeManagementSystemLoginHr.Context
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid") ?? string.Empty;
        public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Name) ?? string.Empty;
        public string UserRole
        {
            get
            {
                var roles = _httpContextAccessor.HttpContext?.User?.Claims?.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value) ;
                if(roles is null) return string.Empty;

                return string.Join(",", roles);
            }
        }
    }



}