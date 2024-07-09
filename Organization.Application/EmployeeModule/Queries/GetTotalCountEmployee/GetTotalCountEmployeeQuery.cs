using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Queries.GetTotalCountEmployee
{
    public sealed record class GetTotalCountEmployeeQuery() : IRequest<int>;
}
