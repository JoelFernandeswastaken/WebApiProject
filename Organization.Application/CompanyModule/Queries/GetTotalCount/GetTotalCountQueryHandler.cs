using ErrorOr;
using MediatR;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Company.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.CompanyModule.Queries.GetTotalCount
{
    public sealed record class GetTotalCountQueryHandler : IRequestHandler<GetTotalCountQuery, ErrorOr<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTotalCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<int>> Handle(GetTotalCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _unitOfWork.Companies.GetTotalCountAsyc(request.Company);
            return count;
        }
    }
}
