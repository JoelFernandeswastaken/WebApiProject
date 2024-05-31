using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using Organization.Application.Common.DTO;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Employee.Models;
using System.Security.Cryptography.X509Certificates;

namespace Organization.Presentation.Api.Controllers
{
    public class EmployeesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public string sqlServerDateFormat = "yyyy-dd-MM HH:mm:ss";
        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var result = await _unitOfWork.Employees.GetAsyncOld();
            return Ok(result);
        }

        [HttpGet]
        [Route("Employee/{id}")]
        public async Task<IActionResult> GetEmployeeByID(string id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if(employee == null)
                return NotFound();
            else 
                return Ok(employee);
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeRequest employee)
        {
            DateTime createdOn, modifiedOn, now;
            string guid = Guid.NewGuid().ToString().Replace("/", "_").Replace("+", "-").Substring(0, 22);
            _unitOfWork.BeginTransaction();
            DateTime.TryParseExact(DateTime.Now.ToString(), sqlServerDateFormat, null, System.Globalization.DateTimeStyles.None, out now);
            var id = _unitOfWork.Employees.AddAsync(new Employee()
            {
                Id = guid,
                Name = employee.name,
                Position = employee.position,   
                CompanyId = employee.companyID,
                CreatedOn = now,
                ModifiedOn = now,  
                Salary = employee.salary,
                Age = employee.age
            });
            _unitOfWork.CommitAndCloseConnection();

            return CreatedAtAction("GetEmployeeByID", new { id = id }, employee);            
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(string id, EmployeeRequest employeeRequest)
        {
            var requiredEmployee = await _unitOfWork.Employees.GetByIdAsync(id);
            if(requiredEmployee == null) 
                return NotFound(employeeRequest);
            else
            {
                if(requiredEmployee.Id != id)
                     return BadRequest(requiredEmployee.Id);

                requiredEmployee.Name = employeeRequest.name;
                requiredEmployee.Age = employeeRequest.age;
                requiredEmployee.CompanyId = employeeRequest.companyID; 
                requiredEmployee.Salary = employeeRequest.salary;
                requiredEmployee.Position = employeeRequest.position;
                requiredEmployee.ModifiedOn = DateTime.Now;

                _unitOfWork.BeginTransaction();
                var result = await _unitOfWork.Employees.UpdateAsync(requiredEmployee);
                _unitOfWork.CommitAndCloseConnection();

                return result ? Ok("Record Updated successfully") : BadRequest("Something went wrong");
            }
        }
        [HttpDelete]
        [Route("DelelteEmployee")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employeeToDelete = _unitOfWork.Employees.GetByIdAsync(id);
            if (employeeToDelete == null)
                return NotFound(employeeToDelete);
            _unitOfWork.BeginTransaction();
            await _unitOfWork.Employees.SoftDeleteAsync(id);
            _unitOfWork.CommitAndCloseConnection();
            return NoContent();
        }
    }
}
