using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;

namespace Organization.Presentation.Api.Controllers.V2
{
    [ApiController]
    [DisableApi]
    // [Route("v2/[controller]")]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class CompaniesController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly ISender _sender; // iSender to send request to handlers
        private readonly IMapper _mapper;
        public CompaniesController(IUnitOfWork unitOfWork, ISender sender, IMapper mapper)
        {
            _unitOfwork = unitOfWork;
            _sender = sender;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all companies(pagination not implemented)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            // await Task.CompletedTask;
            var companies = await _unitOfwork.Companies.GetAsyncV1();
            return Ok(companies);
        }
        /// <summary>
        /// Get all companies
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <resposne code="200">Returns paged list all companies based on query parameters</resposne>
        [HttpGet]
        [Route("GetCompaniesV2")]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyQueryParameters queryParameters)
        {
            var getCompaniesQuery = new GetCompaniesQuery(queryParameters);
            var result = await _sender.Send(getCompaniesQuery);
            return result.Match(
                p => Ok(p),
                errors => Problem(errors)
            );
            // return Ok(result);
        }

        /// <summary>
        /// Return company details based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns code="200">Returns company details</returns>
        /// <exception cref="CompanyNotFoundException"></exception>
        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetCompanyByid(string id)
        {
            var getCompanyByIDQuery = new GetCompanyByIDQuery(id);
            var result = await _sender.Send(getCompanyByIDQuery);
            return result.Match(
                p => Ok(p),
                errors => Problem(errors)
            );
            //if (result == null)
            //{
            //    throw new CompanyNotFoundException("Cound not find company with given ID");
            //}
            //else { return Ok(result); }
        }

        /// <summary>
        /// Add a new company
        /// </summary>
        /// <param name="companyRequest"></param>
        /// <returns code="200">Company added successfully</returns>
        [HttpPost]
        [Route("AddCompany")]
        public async Task<IActionResult> AddCompany(CompanyRequest companyRequest)
        {
            // var addCompanyCommand = new AddCompanyCommand(companyRequest.Name, companyRequest.Address, companyRequest.Country);
            var addCompanyCommand = _mapper.Map<AddCompanyCommand>(companyRequest);
            var id = await _sender.Send(addCompanyCommand);
            return id.Match(
                p => CreatedAtAction("GetCompanyByid", new { p }, companyRequest),
                errors => Problem(errors)
            );
            // return CreatedAtAction("GetCompanyByid", new { id }, companyRequest);

        }

        /// <summary>
        /// Update a company based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyRequest"></param>
        /// <returns code="200">Company updated successfully</returns>
        /// <returns code="400">Bad request</returns>
        [HttpPut]
        [Route("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany(string id, CompanyRequest companyRequest)
        {
            // var updateCompanyCommmand = new UpdateCompanyCommand(id, companyRequest.Name, companyRequest.Address, companyRequest.Country);
            var updateCompanyCommand = _mapper.Map<UpdateCompanyCommand>((id, companyRequest));
            var result = await _sender.Send(updateCompanyCommand);
            return result.Match(
                p => Ok("Record updated successfully"),
                errors => Problem(errors)
            );
            // return result ? Ok("Record Updated successfully") : BadRequest("Something went wrong");
        }

        /// <summary>
        /// Delete company based on ID
        /// </summary>
        /// <param name="id">ID of company to be updated</param>
        /// <param name="deleteAssociations">Delete associated employees</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteComany")]
        public async Task<IActionResult> DeleteCompany(string id, bool deleteAssociations)
        {
            // var deleteCompanyCommand = new DeleteCompanyCommand(id, deleteAssociations);
            var deleteCompanyCommand = _mapper.Map<DeleteCompanyCommand>((id, deleteAssociations));
            var rowsAffected = await _sender.Send(deleteCompanyCommand);
            return rowsAffected.Match(
                p => Ok($"{rowsAffected} rows affected"),
                errors => Problem(errors)
            );
            //if (rowsAffected == 0)
            //    return Ok("No rows Affected");
            //else if (rowsAffected > 0)
            //    return Ok($"{rowsAffected} rows Affected");
            //else
            //    return BadRequest("Bad Request");

        }

        /// <summary>
        /// Get total count of Companies
        /// </summary>
        /// <returns code="200">Total count of companies</returns>
        [HttpGet]
        [Route("GetTotalCount")]
        public async Task<IActionResult> GetTotalCount()
        {
            var company = new Company();
            var getTotalCountQuery = new GetTotalCountQuery(company);
            var count = await _sender.Send(getTotalCountQuery);
            return count.Match(
                p => Ok(count),
                errors => Problem(errors)
            );
            // return Ok(count);
        }
    }
}
