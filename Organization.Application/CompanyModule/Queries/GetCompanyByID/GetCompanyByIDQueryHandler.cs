using MediatR;
using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Organization.Domain.Common.Errors;

namespace Organization.Application.CompanyModule.Queries.GetCompanyByID
{
    public sealed class GetCompanyByIDQueryHandler : IRequestHandler<GetCompanyByIDQuery, ErrorOr<CompanyResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCompanyByIDQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<CompanyResponse>> Handle(GetCompanyByIDQuery request, CancellationToken cancellationToken)
        {
            var companyByID = await _unitOfWork.Companies.GetByIdAsync(request.id);
            if (companyByID == null)
                return Errors.Company.CompanyDoesNotExist($"Company with given ID {request.id} does not exist");
            CompanyResponse response = new CompanyResponse(companyByID.Id, companyByID.Name, companyByID.Address, companyByID.Country);
            return response;
        }
    }
}
