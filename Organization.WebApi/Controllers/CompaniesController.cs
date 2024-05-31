using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using Organization.Application.Common.DTO;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Utilities;
using Organization.Domain.Company.Models;
using Organization.Infrastructure.Persistance;
using System.Runtime.InteropServices;

namespace Organization.Presentation.Api.Controllers
{
    [ApiController]
    [DisableApi]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;
        public CompaniesController(IUnitOfWork unitOfWork) 
        {
            _unitOfwork = unitOfWork;
        }
        [HttpGet]
        [Route("/GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                // await Task.CompletedTask;
                var companies = await _unitOfwork.Companies.GetAsyncOld();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet("/company/{id}")]
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
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("/AddCompany")]
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

                return CreatedAtAction("GetCompanyByid", new { id = id }, companyRequest);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPut]
        [Route("/UpdateCompany")]
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
                return BadRequest(ex.Message);
            }
            
        }
        [HttpDelete]
        [Route("/DeleteComany")]
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
                await _unitOfwork.Companies.SoftDeleteAsync(deleteCompany.Id, deleteAssociations);
                _unitOfwork.CommitAndCloseConnection();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
