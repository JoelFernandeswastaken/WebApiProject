using Microsoft.AspNetCore.Mvc;
using Organization.Application.Common.DTO;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Company.Models;
using Organization.Infrastructure.Persistance;

namespace Organization.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly List<Company> _companies;
        private readonly IUnitOfWork _unitOfwork;
        public CompaniesController(IUnitOfWork unitOfWork) 
        {
            _companies = new List<Company>()
            {
                new Company() {Id = "TestId123", Name = "Company1"},
                new Company() {Id = "TestId456", Name = "Company2"},
                new Company() {Id = "TestId789", Name = "Company3"},
            };
            _unitOfwork = unitOfWork;
        }
        [HttpGet]
        [Route("/GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            // await Task.CompletedTask;
            var companies = await _unitOfwork.Companies.GetAsyncOld();
            return Ok(companies);
        }
        [HttpGet("/company/{id}")]
        public async Task<IActionResult> GetCompanyByid(string id)
        {
            var compayByID = await _unitOfwork.Companies.GetByIdAsync(id);
            if (compayByID == null)
            {
                return NotFound(compayByID);
            }
            else { return Ok(compayByID); }
        }
        [HttpPost]
        [Route("/AddCompany")]
        public async Task<IActionResult> AddCompany(CompanyRequest companyRequest)
        {
            try
            {
                //if(company.Id == null || company.Name == null) 
                //{
                //    return BadRequest(company);
                //}
                //else
                //{
                //    _companies.Add(company);
                //    return CreatedAtAction("GetCompanyByid", new { id = company.Id }, company);
                //}
                string guid = Guid.NewGuid().ToString().Replace("/", "_").Replace("+", "-").Substring(0, 22);
                _unitOfwork.BeginTransaction();
                var added = await _unitOfwork.Companies.AddAsync(new Company()
                {
                    Name = companyRequest.Name,
                    Address = companyRequest.Address,
                    Country = companyRequest.Country
                });
                _unitOfwork.CommitAndCloseConnection();
                return Ok(added);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPut]
        [Route("/UpdateCompany")]
        public async Task<IActionResult> UpdateCompany(string id, string name)
        {
            var requiredCompany = _companies.Find(x => x.Id == id);
            if(requiredCompany == null)
            {
                return NotFound();
            }
            else
            {
                requiredCompany.Name = name;    
                return Ok(_companies);
            }
        }
        [HttpDelete]
        [Route("/DeleteComany")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            var deleteCompany = _companies.Find(x => x.Id == id);
            if (deleteCompany == null)
            {
                return NotFound();
            }
            else
            {
                _companies.Remove(deleteCompany);
                // return Ok(_companies);
                return NoContent ();    
            }
        }
    }
}
