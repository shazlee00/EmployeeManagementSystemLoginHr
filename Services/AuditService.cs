using AutoMapper;
using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.UOW;

namespace EmployeeManagementSystemLoginHr.Services
{
    public class AuditService : IAuditService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditService(UnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<List<AuditLogDto>> GetAuditsAsync()
        {
            var audits = await _unitOfWork.AuditLogRepository.GetAllAsync();
            return _mapper.Map<List<AuditLogDto>>(audits);
        }


    }

}
