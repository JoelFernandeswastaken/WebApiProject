using Organization.Application.Common.DTO;
using Organization.Application.Common.Utilities;
using Organization.Domain.Company.Models;
using Organization.Domain.Employee;
using Organization.Domain.Employee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Interfaces.Persistance
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public Task<PageList<EmployeeResponse>> GetEmployeesByQueryAsyc(EmployeeQueryParameters queryParameters);
    }
}
