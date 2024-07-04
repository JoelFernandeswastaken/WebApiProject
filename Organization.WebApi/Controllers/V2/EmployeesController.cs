using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using Organization.Application.Common.DTO.Request;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Utilities;
using Organization.Domain.Employee;
using Organization.Domain.Employee.Models;
using System.Security.Cryptography.X509Certificates;

namespace Organization.Presentation.Api.Controllers.V2
{
    [ApiController]
    [DisableApi]
    // [Route("v2/[controller]")]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiVersion("2.0")]

    public class EmployeesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public string sqlServerDateFormat = "yyyy-dd-MM HH:mm:ss";
        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all employees. Implemented without pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetEmployeesV1()
        {
            try
            {
                var result = await _unitOfWork.Employees.GetAsyncV1();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeesV2")]
        public async Task<IActionResult> GetEmployeesV2([FromQuery] EmployeeQueryParameters queryParameters)
        {
            try
            {
                var result = await _unitOfWork.Employees.GetEmployeesByQueryAsyc(queryParameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("Employee/{id}")]
        public async Task<IActionResult> GetEmployeeByID(string id)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id);
                if (employee == null)
                    return NotFound();
                else
                    return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeRequest employee)
        {
            try
            {
                string guid = Guid.NewGuid().ToString().Replace("/", "_").Replace("+", "-").Substring(0, 22);

                _unitOfWork.BeginTransaction();
                var id = await _unitOfWork.Employees.AddAsync(new Employee()
                {
                    Id = guid,
                    Name = employee.name,
                    Position = employee.position,
                    CompanyId = employee.companyID,
                    CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    Salary = employee.salary,
                    Age = employee.age
                });
                _unitOfWork.CommitAndCloseConnection();

                if (id == guid)
                    return CreatedAtAction("GetEmployeeByID", new { id }, employee);
                else
                    return BadRequest("Something went wrong");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(string id, EmployeeRequest employeeRequest)
        {
            try
            {
                var requiredEmployee = await _unitOfWork.Employees.GetByIdAsync(id);

                if (requiredEmployee == null)
                    return NotFound(employeeRequest);
                else
                {
                    if (requiredEmployee.Id != id)
                        return BadRequest(requiredEmployee.Id);

                    requiredEmployee.Name = employeeRequest.name;
                    requiredEmployee.Age = employeeRequest.age;
                    requiredEmployee.CompanyId = employeeRequest.companyID;
                    requiredEmployee.Salary = employeeRequest.salary;
                    requiredEmployee.Position = employeeRequest.position;
                    requiredEmployee.ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    _unitOfWork.BeginTransaction();
                    var result = await _unitOfWork.Employees.UpdateAsync(requiredEmployee);
                    _unitOfWork.CommitAndCloseConnection();

                    return result ? Ok("Record Updated successfully") : BadRequest("Something went wrong");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Route("DelelteEmployee")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            try
            {
                var employeeToDelete = _unitOfWork.Employees.GetByIdAsync(id);
                if (employeeToDelete == null)
                    return NotFound(employeeToDelete);

                _unitOfWork.BeginTransaction();
                int rowsAffected = await _unitOfWork.Employees.SoftDeleteAsync(id);
                _unitOfWork.CommitAndCloseConnection();

                if (rowsAffected == 0)
                    return Ok("No rows Affected");
                else if (rowsAffected > 0)
                    return Ok($"{rowsAffected} rows Affected");
                else
                    return BadRequest("Bad Request");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetTotalCount")]
        public async Task<IActionResult> GetTotalCount()
        {
            try
            {
                var employee = new Employee();
                int count = await _unitOfWork.Employees.GetTotalCountAsyc(employee);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
