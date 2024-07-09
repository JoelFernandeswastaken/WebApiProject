using MediatR;
using Organization.Application.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Queries.GetEmployeeByID
{
    public sealed record class GetEmployeeByIDQuery(string id) : IRequest<EmployeeResponse>;
}
