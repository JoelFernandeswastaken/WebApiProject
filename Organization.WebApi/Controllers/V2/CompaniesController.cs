using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using Organization.Application.Common.DTO.Request;
using Organization.Application.Common.Exceptions;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Application.CompanyModule.Commands.AddCompany;
using Organization.Application.CompanyModule.Commands.DeleteCompany;
using Organization.Application.CompanyModule.Commands.UpdateCompany;
using Organization.Application.CompanyModule.Queries.GetCompanies;
using Organization.Application.CompanyModule.Queries.GetCompanyByID;
using Organization.Application.CompanyModule.Queries.GetTotalCount;
using Organization.Domain.Common.Utilities;
using Organization.Domain.Company;
using Organization.Domain.Company.Models;
using Organization.Domain.Employee.Models;
using Organization.Infrastructure.Persistance;
using System.Runtime.InteropServices;

namespace Organization.Presentation.Api.Controllers.V2
{
    [ApiController]
    [DisableApi]
    // [Route("v2/[controller]")]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class CompaniesController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly ISender _sender; // iSender to send request to handlers
        public CompaniesController(IUnitOfWork unitOfWork, ISender sender)
        {
            _unitOfwork = unitOfWork;
            _sender = sender;
        }
        [HttpGet]
        [Route("GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            // await Task.CompletedTask;
            var companies = await _unitOfwork.Companies.GetAsyncV1();
            return Ok(companies);
        }
        [HttpGet]
        [Route("GetCompaniesV2")]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyQueryParameters queryParameters)
        {
            var getCompaniesQuery = new GetCompaniesQuery(queryParameters);
            var result = await _sender.Send(getCompaniesQuery);
            return Ok(result);
        }
        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetCompanyByid(string id)
        {
            var getCompanyByIDQuery = new GetCompanyByIDQuery(id);
            var result = await _sender.Send(getCompanyByIDQuery);
            if (result == null)
            {
                throw new CompanyNotFoundException("Cound not find company with given ID");
            }
            else { return Ok(result); }
        }
        [HttpPost]
        [Route("AddCompany")]
        public async Task<IActionResult> AddCompany(CompanyRequest companyRequest)
        {
            var addCompanyCommand = new AddCompanyCommand(companyRequest.Name, companyRequest.Address, companyRequest.Country);
            var id = _sender.Send(addCompanyCommand);
            return CreatedAtAction("GetCompanyByid", new { id }, companyRequest);

        }
        [HttpPut]
        [Route("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany(string id, CompanyRequest companyRequest)
        {
            var updateCompanyCommmand = new UpdateCompanyCommand(id, companyRequest.Name, companyRequest.Address, companyRequest.Country);
            var result = await _sender.Send(updateCompanyCommmand);

            //var requiredCompany = await _unitOfwork.Companies.GetByIdAsync(id);
            //if (requiredCompany == null)
            //{
            //    return NotFound(requiredCompany);
            //}
            //if (requiredCompany.Id != id)
            //{
            //    return BadRequest(requiredCompany);
            //}
            //requiredCompany.Name = companyRequest.Name;
            //requiredCompany.Address = companyRequest.Address;
            //requiredCompany.Country = companyRequest.Country;


            //_unitOfwork.BeginTransaction();
            //bool result = await _unitOfwork.Companies.UpdateAsync(requiredCompany);
            //_unitOfwork.CommitAndCloseConnection();

            return result ? Ok("Record Updated successfully") : BadRequest("Something went wrong");
        }
        [HttpDelete]
        [Route("DeleteComany")]
        public async Task<IActionResult> DeleteCompany(string id, bool deleteAssociations)
        {
            var deleteCompanyCommand = new DeleteCompanyCommand(id, deleteAssociations);
            var rowsAffected = await _sender.Send(deleteCompanyCommand);    

            if (rowsAffected == 0)
                return Ok("No rows Affected");
            else if (rowsAffected > 0)
                return Ok($"{rowsAffected} rows Affected");
            else
                return BadRequest("Bad Request");

        }
        [HttpGet]
        [Route("GetTotalCount")]
        public async Task<IActionResult> GetTotalCount()
        {
            var company = new Company();
            var getTotalCountQuery = new GetTotalCountQuery(company);
            var count = await _sender.Send(getTotalCountQuery);
            return Ok(count);
        }
    }
}
