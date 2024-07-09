using MediatR;
using Organization.Domain.Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.CompanyModule.Queries.GetTotalCount
{
    public sealed record class GetTotalCountQuery(Company Company) : IRequest<int>;
}
