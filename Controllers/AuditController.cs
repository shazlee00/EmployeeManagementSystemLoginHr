using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EmployeeManagementSystemLoginHr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;
        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }
        [SwaggerOperation(Summary = "Retrieves a list of audit logs", Description = "Returns all audit records from the system.")]
        [SwaggerResponse(200, "List of audits", typeof(IEnumerable<AuditLogDto>))]
        [SwaggerResponse(500, "Internal Server Error")]
        [HttpGet]
        public async Task<IActionResult> GetAuditsAsync()
        {
            var audits = await _auditService.GetAuditsAsync();
            return Ok(audits);
        }



    }
}
