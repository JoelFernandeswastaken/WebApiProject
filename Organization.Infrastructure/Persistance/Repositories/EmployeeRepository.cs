using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Employee.Models;
using Organization.Infrastructure.Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
        }
    }
}
