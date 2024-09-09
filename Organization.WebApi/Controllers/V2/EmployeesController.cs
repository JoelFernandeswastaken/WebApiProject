using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using Organization.Application.Common.DTO.Request;
using Organization.Application.Common.Exceptions;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Application.CompanyModule.Commands.UpdateCompany;
using Organization.Application.CompanyModule.Queries.GetTotalCount;
using Organization.Application.EmployeeModule.Commands.AddEmployee;
using Organization.Application.EmployeeModule.Commands.DeleteEmployee;
using Organization.Application.EmployeeModule.Commands.UpdateEmployee;
using Organization.Application.EmployeeModule.Queries.GetEmployeeByID;
using Organization.Application.EmployeeModule.Queries.GetEmployees;
using Organization.Application.EmployeeModule.Queries.GetTotalCountEmployee;
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

    public class EmployeesController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        public string sqlServerDateFormat = "yyyy-dd-MM HH:mm:ss";
        public EmployeesController(IUnitOfWork unitOfWork, ISender sender, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _sender = sender;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all employees. Implemented without pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetEmployeesV1()
        {
            var result = await _unitOfWork.Employees.GetAsyncV1();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetEmployeesV2")]
        public async Task<IActionResult> GetEmployeesV2([FromQuery] EmployeeQueryParameters queryParameters)
        {
            var getEmployeesQuery = new GetEmployeesQuery(queryParameters);
            var result = await _sender.Send(getEmployeesQuery);
            return result.Match(
                p => Ok(p),
                errors => Problem(errors)
            );

            // return Ok(result);
        }

        [HttpGet]
        [Route("Employee/{id}")]
        public async Task<IActionResult> GetEmployeeByID(string id)
        {
            var getEmployeeByIDQuery = new GetEmployeeByIDQuery(id);
            var result = await _sender.Send(getEmployeeByIDQuery);
            return result.Match(
                p => Ok(p),
                errors => Problem(errors)
            );

            //if (result == null)
            //    throw new EmployeeNotFoundException($"Could not find employee with given ID: {id}"); 
            //else
            //    return Ok(result);
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeRequest employee)
        {
            // var addEmployeeCommand = new AddEmployeeCommand(employee.age, employee.name, employee.position, employee.companyID, employee.salary);
            var addEmployeeCommand = _mapper.Map<AddEmployeeCommand>(employee);
            var result = await _sender.Send(addEmployeeCommand);
            return result.Match(
                p => CreatedAtAction("GetEmployeeByID", new { p }, employee),
                errors => Problem(errors)
            );
            // return CreatedAtAction("GetEmployeeByID", new { id }, employee);
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(string id, EmployeeRequest employeeRequest)
        {
            // var updateEmployeeCommand = new UpdateEmployeeCommand(id, employeeRequest.Age, employeeRequest.Name, employeeRequest.Position, employeeRequest.CompanyID, employeeRequest.Salary);
            var updateEmployeeCommand = _mapper.Map<UpdateEmployeeCommand>((id, employeeRequest));
            var result =  await _sender.Send(updateEmployeeCommand);

            return result.Match(
                p => Ok("Employee updated sucessfully"),
                errors => Problem(errors)   
            );
            // return result ? Ok("Record Updated successfully") : throw new Exception("Something went wrong");

        }
        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var deleteEmployeeCommand = new DeleteEmployeeCommand(id);
            var rowsAffected = await _sender.Send(deleteEmployeeCommand);

            return rowsAffected.Match(
                p => Ok($"{p} rows affected"),
                errors => Problem(errors)   
            );

            //if (rowsAffected == 0)
            //    return Ok("No rows Affected");
            //else if (rowsAffected > 0)
            //    return Ok($"{rowsAffected} rows Affected");
            //else
            //    throw new Exception("Employee Delete operation failed");
        }
        [HttpGet]
        [Route("GetTotalCount")]
        public async Task<IActionResult> GetTotalCount()
        {
            var getTotalCountEmployeeQuery = new GetTotalCountEmployeeQuery();
            var result = await _sender.Send(getTotalCountEmployeeQuery);
            return result.Match(
                p => Ok(p),
                errors => Problem(errors)
            );

            // return Ok(count);
        }
    }
}
