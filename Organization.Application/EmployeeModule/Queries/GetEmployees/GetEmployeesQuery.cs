using MediatR;
using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Utilities;
using Organization.Domain.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace Organization.Application.EmployeeModule.Queries.GetEmployees
{
    public sealed record class GetEmployeesQuery(EmployeeQueryParameters parameters) : IRequest<ErrorOr<PageList<EmployeeResponse>>>;
}
