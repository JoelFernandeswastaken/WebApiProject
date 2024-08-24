using MediatR;
using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Application.Common.Utilities;
using ErrorOr;

namespace Organization.Application.CompanyModule.Queries.GetCompanies
{
    public sealed class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, ErrorOr<PageList<CompanyResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCompaniesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<PageList<CompanyResponse>>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Companies.GetCompaniesByQueryAsync(request.companyQueryParameters);
            return result;
        }
    }
}
