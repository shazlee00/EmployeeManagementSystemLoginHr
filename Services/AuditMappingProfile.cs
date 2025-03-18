using AutoMapper;
using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.Models;

namespace EmployeeManagementSystemLoginHr.Services
{
    public class AuditMappingProfile:Profile
    {
        public AuditMappingProfile()
        {
            CreateMap<AuditLog, AuditLogDto>().ReverseMap();
        }
    }

}
