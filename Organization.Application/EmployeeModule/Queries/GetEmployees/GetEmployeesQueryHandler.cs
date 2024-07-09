using MediatR;
using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Application.Common.Utilities;
using Organization.Domain.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Queries.GetEmployees
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, PageList<EmployeeResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEmployeesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageList<EmployeeResponse>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetEmployeesByQueryAsyc(request.parameters);
            return result;
        }
    }
}
