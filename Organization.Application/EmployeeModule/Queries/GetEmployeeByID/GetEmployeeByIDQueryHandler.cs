using MediatR;
using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Interfaces.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Queries.GetEmployeeByID
{
    public class GetEmployeeByIDQueryHandler : IRequestHandler<GetEmployeeByIDQuery, EmployeeResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEmployeeByIDQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeResponse> Handle(GetEmployeeByIDQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
