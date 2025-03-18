using EmployeeManagementSystemLoginHr.Dtos;

namespace EmployeeManagementSystemLoginHr.Services
{
    public interface IAuditService
    {
        Task<List<AuditLogDto>> GetAuditsAsync();
    }

}
