using ErrorOr;
using MediatR;
using Organization.Domain.Company.Models;

namespace Organization.Application.CompanyModule.Queries.GetTotalCount
{
    public sealed record class GetTotalCountQuery(Company Company) : IRequest<ErrorOr<int>>;
}
