using MediatR;
using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Utilities;
using Organization.Domain.Company;
using ErrorOr;

namespace Organization.Application.CompanyModule.Queries.GetCompanies
{
    public sealed record class GetCompaniesQuery(CompanyQueryParameters companyQueryParameters) : IRequest<ErrorOr<PageList<CompanyResponse>>>;
}
