using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Utilities;
using Organization.Domain.Company;
using Organization.Domain.Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Interfaces.Persistance
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        public Task<PageList<CompanyResponse>> GetCompaniesByQueryAsync(CompanyQueryParameters queryParameters);
    }
}
