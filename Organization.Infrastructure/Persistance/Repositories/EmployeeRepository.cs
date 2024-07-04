using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Application.Common.Utilities;
using Organization.Domain.Employee;
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

        public async Task<PageList<EmployeeResponse>> GetEmployeesByQueryAsyc(EmployeeQueryParameters queryParameters)
        {
            var employees = (await GetAsyncV2(queryParameters, "Name", "Age", "Position", "Salary", "CreatedOn")).AsQueryable().Select(s => new EmployeeResponse { 
                Name = s.Name,
                Age = s.Age,
                Salary = (int)s.Salary,
                CreatedOn = s.CreatedOn,
            });

            if(!string.IsNullOrEmpty(queryParameters.EmployeeName))
                employees = employees.Where(s => s.Name.ToLowerInvariant().Contains(queryParameters.EmployeeName.ToLowerInvariant()));

            var pagedEmployees = PageList<EmployeeResponse>.Create(employees, queryParameters.PageNo, queryParameters.PageSize, 10000);
            return pagedEmployees;
        }
    }
}
