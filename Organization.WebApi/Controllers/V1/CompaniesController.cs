using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using Organization.Application.Common.DTO;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Application.Common.Utilities;
using Organization.Domain.Common.Utilities;
using Organization.Domain.Company;
using Organization.Domain.Company.Models;
using Organization.Infrastructure.Persistance;
using Organization.Presentation.Api.Swagger.Examples.Response;
using Swashbuckle.AspNetCore.Filters;
using System.Runtime.InteropServices;

namespace Organization.Presentation.Api.Controllers.V1
{
    [ApiController]
    [DisableApi]
    // [Route("[controller]")]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CompaniesController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;
        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfwork = unitOfWork;
        }
        /// <summary>
        /// Get all companies without pagination.
        /// </summary>
        /// <response code="200">Returns list of all companies</response>
        [HttpGet]
        [Route("GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                // await Task.CompletedTask;
                var companies = await _unitOfwork.Companies.GetAsyncV1();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        /// <summary>
        /// Get companies based on some query parameters with pagination.
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <resposne code="200">Returns paged list all companies based on query parameters</resposne>
        [HttpGet]
        [Route("GetCompaniesV2")]
        [SwaggerRequestExample(typeof(PageList<CompanyResponse>), typeof(GetCompaniesV2ResponseExample))]
        // [ProducesResponseType(typeof(GetCompaniesV2ResponseExample), 200)]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyQueryParameters queryParameters)
        {
            try
            {

                var result = await _unitOfwork.Companies.GetCompaniesByQueryAsync(queryParameters);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        /// <summary>
        /// Get company based on ID.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns company details based on ID</response>
        /// <response code="404">Could not find the company</response>
        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetCompanyByid(string id)
        {
            try
            {
                var compayByID = await _unitOfwork.Companies.GetByIdAsync(id);
                if (compayByID == null)
                {
                    return NotFound(compayByID);
                }
                else { return Ok(compayByID); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Add a new company
        /// </summary>
        /// <param name="companyRequest">CompanyRequest</param>
        /// <response code="200">Company added successfully</response>
        [HttpPost]
        [Route("AddCompany")]
        public async Task<IActionResult> AddCompany(CompanyRequest companyRequest)
        {
            try
            {
                string guid = Guid.NewGuid().ToString().Replace("/", "_").Replace("+", "-").Substring(0, 22);

                _unitOfwork.BeginTransaction();
                var id = _unitOfwork.Companies.AddAsync(new Company()
                {
                    Id = guid,
                    Name = companyRequest.Name,
                    Address = companyRequest.Address,
                    Country = companyRequest.Country
                }); //Alter stored procedure to return id of company added
                _unitOfwork.CommitAndCloseConnection();

                return CreatedAtAction("GetCompanyByid", new { id }, companyRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        /// <summary>
        /// Update a company's details.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyRequest"></param>
        /// <response code="200">Company updated</response>
        [HttpPut]
        [Route("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany(string id, CompanyRequest companyRequest)
        {
            try
            {
                var requiredCompany = await _unitOfwork.Companies.GetByIdAsync(id);
                if (requiredCompany == null)
                {
                    return NotFound(requiredCompany);
                }
                if (requiredCompany.Id != id)
                {
                    return BadRequest(requiredCompany);
                }
                requiredCompany.Name = companyRequest.Name;
                requiredCompany.Address = companyRequest.Address;
                requiredCompany.Country = companyRequest.Country;

                _unitOfwork.BeginTransaction();
                bool result = await _unitOfwork.Companies.UpdateAsync(requiredCompany);
                _unitOfwork.CommitAndCloseConnection();

                return result ? Ok("Record Updated successfully") : BadRequest("Something went wrong");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        /// <summary>
        /// Delete company based on ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleteAssociations"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteComany")]
        public async Task<IActionResult> DeleteCompany(string id, bool deleteAssociations)
        {
            try
            {
                var deleteCompany = await _unitOfwork.Companies.GetByIdAsync(id);
                if (deleteCompany == null)
                {
                    return NotFound(deleteCompany);
                }
                if (deleteCompany.Id != id)
                {
                    return BadRequest(deleteCompany);
                }
                _unitOfwork.BeginTransaction();
                int rowsAffected = await _unitOfwork.Companies.SoftDeleteAsync(deleteCompany.Id, deleteAssociations);
                _unitOfwork.CommitAndCloseConnection();

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
    }
}
