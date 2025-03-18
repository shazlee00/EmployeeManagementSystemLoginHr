using AutoMapper;
using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.Enums;
using EmployeeManagementSystemLoginHr.Helpers;
using EmployeeManagementSystemLoginHr.Models;
using EmployeeManagementSystemLoginHr.ResourceParameters;
using EmployeeManagementSystemLoginHr.UOW;
using System;

namespace EmployeeManagementSystemLoginHr.Services
{
    public class EmployeeService:IEmployeeService
    {
            private readonly UnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public EmployeeService(UnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
            {

                if (await _unitOfWork.EmployeeRepository.EmployeeCodeExistsAsync(dto.EmployeeCode))
                        throw new Exception("Employee code must be unique.");
                if (await _unitOfWork.EmployeeRepository.AnyAsync(e => e.Email == dto.Email))
                    throw new Exception("Email must be unique.");

                if (await _unitOfWork.EmployeeRepository.AnyAsync(e => e.NationalId == dto.NationalId))
                    throw new Exception("National Id must be unique.");

                var employee = _mapper.Map<Employee>(dto);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<EmployeeDto>(employee);
            }

            public async Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id) ?? throw new Exception("Employee not found.");
                _mapper.Map(dto, employee);
                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<EmployeeDto>(employee);
            }

            public async Task<PagedList<EmployeeDto>> GetEmployeesAsync(EmployeeParameters employeeParameters)
            {
                ArgumentNullException.ThrowIfNull(employeeParameters);
                var employees = await _unitOfWork.EmployeeRepository.GetFilteredEmployees(employeeParameters);

                PagedList<EmployeeDto> pagedList = new(employees.Select(e => _mapper.Map<EmployeeDto>(e)).ToList(), 
                                                        employees.TotalCount, employeeParameters.PageNumber, employeeParameters.PageSize);
                

                    
                  return pagedList;
                 
      
               
            }


       
        public async Task DeleteEmployeeAsync(int id)
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id) ;
                if (employee.EmploymentStatus == EmploymentStatus.Active) throw new Exception("Cannot delete an active employee.");
                await _unitOfWork.EmployeeRepository.RemoveAsync(employee);
                await _unitOfWork.CompleteAsync();
            }


        public async Task<EmployeeDto> GetEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            return _mapper.Map<EmployeeDto>(employee);
        }
        public async Task<bool> CheckEmployeeCodeExistsAsync(string employeeCode)
        {
            var exists = await _unitOfWork.EmployeeRepository.EmployeeCodeExistsAsync(employeeCode);
            return exists;

        }

    }
}
