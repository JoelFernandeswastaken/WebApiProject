using MediatR;
using Organization.Application.Common.DTO.Response;
using Organization.Domain.Company.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace Organization.Application.CompanyModule.Queries.GetCompanyByID
{
    public sealed record class GetCompanyByIDQuery(string id) : IRequest<ErrorOr<CompanyResponse>>;
}
