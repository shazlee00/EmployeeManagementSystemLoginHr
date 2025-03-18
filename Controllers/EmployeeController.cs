using AutoMapper;
using EmployeeManagementSystemLoginHr.Authorization;
using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.Helpers;
using EmployeeManagementSystemLoginHr.ResourceParameters;
using EmployeeManagementSystemLoginHr.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EmployeeManagementSystemLoginHr.Controllers
{
    /// <summary>
    /// Manages Employee operations including Create, Read, Update, and Delete.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEmployeeDto> _createValidator;
        private readonly IValidator<UpdateEmployeeDto> _updateValidator;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper,IValidator<CreateEmployeeDto> validator, IValidator<UpdateEmployeeDto> updateValidator)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _updateValidator = updateValidator;
            _createValidator = validator;
        }

        //endpoint summary



        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="dto">Employee creation details.</param>
        /// <returns>Created employee details.</returns>
        [HttpPost]
        [CheckPermission(Permissions.EmployeePermissions.Create)]
        [SwaggerOperation(Summary = "Creates a new employee", Description = "Requires Create permission.")]
        [SwaggerResponse(201, "Employee created successfully", typeof(EmployeeDto))]
        [SwaggerResponse(400, "Invalid request data")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) 
                return BadRequest(new
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest,
                    errors = validationResult.Errors
                });
            var employee = await _employeeService.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);        
            
        }

        /// <summary>
        /// Retrieves an employee by ID.
        /// </summary>
        /// <param name="id">Employee ID.</param>
        /// <returns>Employee details.</returns>
        [SwaggerOperation(Summary = "Gets an employee by ID", Description = "Requires View permission.")]
        [SwaggerResponse(200, "Employee retrieved successfully", typeof(EmployeeDto))]
        [SwaggerResponse(404, "Employee not found")]
        [HttpGet("{id}")]
        [CheckPermission(Permissions.EmployeePermissions.View)]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            if (employee == null)
                return NotFound(new { error = "Employee not found." });
            return Ok(employee);
        }
        /// <summary>
        /// Retrieves a paginated list of employees.
        /// </summary>
        /// <param name="employeeParameters">Pagination and filtering parameters.</param>
        /// <returns>List of employees.</returns>
        [HttpGet]
        [CheckPermission(Permissions.EmployeePermissions.View)]
        [SwaggerOperation(Summary = "Gets a list of employees", Description = "Supports pagination and filtering.")]
        [SwaggerResponse(200, "Employees retrieved successfully", typeof(PagedList<EmployeeDto>))]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeParameters employeeParameters)
        {


            var employees = await _employeeService.GetEmployeesAsync(employeeParameters);
            var metadata = new
            {
                employees.TotalCount,
                employees.PageSize,
                employees.CurrentPage,
                employees.TotalPages,
                employees.HasNext,
                employees.HasPrevious
            };
            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(metadata));
            return Ok(employees);
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">Employee ID.</param>
        /// <param name="dto">Updated employee details.</param>
        /// <returns>Updated employee information.</returns>

        [HttpPut("{id}")]
        [CheckPermission(Permissions.EmployeePermissions.Update)]
        [SwaggerOperation(Summary = "Updates an existing employee", Description = "Requires Update permission.")]
        [SwaggerResponse(200, "Employee updated successfully", typeof(EmployeeDto))]
        [SwaggerResponse(400, "Invalid request data")]
        [SwaggerResponse(404, "Employee not found")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            if(id != dto.Id) return BadRequest();

            if(await _employeeService.GetEmployeeAsync(id) == null) 
                return NotFound(new { error = "Employee not found." });
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) return BadRequest(new
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                errors = validationResult.Errors
            });
            var employee = await _employeeService.UpdateEmployeeAsync(id, dto);
            return Ok(employee);
        }
        /// <summary>
        /// Deletes an employee.
        /// </summary>
        /// <param name="id">Employee ID.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [CheckPermission(Permissions.EmployeePermissions.Delete)]
        [SwaggerOperation(Summary = "Deletes an employee", Description = "Requires Delete permission.")]
        [SwaggerResponse(204, "Employee deleted successfully")]
        [SwaggerResponse(404, "Employee not found")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if(await _employeeService.GetEmployeeAsync(id) == null) 
                return NotFound(new { error = "Employee not found." });
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
            return NoContent(); 
        }












    }
}
