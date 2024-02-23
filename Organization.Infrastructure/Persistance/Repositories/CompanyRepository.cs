using Organization.Application.Common.Interfaces.Persistance;
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
    }
}
