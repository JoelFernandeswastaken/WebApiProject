using Microsoft.AspNetCore.Mvc;
using Organization.Domain.Company.Models;

namespace Organization.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly List<Company> _companies;
        public CompaniesController() 
        {
            _companies = new List<Company>()
            {
                new Company() {Id = "TestId123", Name = "Company1"},
                new Company() {Id = "TestId456", Name = "Company2"},
                new Company() {Id = "TestId789", Name = "Company3"},
            };
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            await Task.CompletedTask;
            return Ok(_companies);
        }
        [HttpGet("/company/{id:Length(9)}")]
        public async Task<IActionResult> GetCompanyByid(string id)
        {
            var found = _companies.Find(x => x.Id == id);
            if (found == null)
            {
                return NotFound(found);
            }
            else { return Ok(found); }
        }
        [HttpPost]
        public async Task<IActionResult> AddCompany([FromForm] Company company)
        {         
            if(company.Id == null || company.Name == null) 
            {
                return BadRequest(company);
            }
            else
            {
                _companies.Add(company);
                return CreatedAtAction("GetCompanyByid", new { id = company.Id }, company);
            }
        }
        [HttpPut]
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
