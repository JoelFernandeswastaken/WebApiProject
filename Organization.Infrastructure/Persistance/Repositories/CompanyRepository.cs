using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Application.Common.Utilities;
using Organization.Domain.Company;
using Organization.Domain.Company.Models;
using Organization.Infrastructure.Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {

        }
        public async Task<PageList<CompanyResponse>> GetCompaniesByQueryAsync(CompanyQueryParameters queryParameters)
        {
            var companies = (await GetAsyncV2(queryParameters)).AsQueryable().Select(s => new CompanyResponse(s.Id, s.Name, s.Country, s.Address));

            if (!string.IsNullOrEmpty(queryParameters.CompanyName))
                companies = companies.Where(s => s.Name.ToLowerInvariant().Contains(queryParameters.CompanyName.ToLowerInvariant()));


            var pagedCompanies = PageList<CompanyResponse>.Create(companies, queryParameters.PageNo, queryParameters.PageSize, 10000);

            return pagedCompanies;
        }
    }
}
