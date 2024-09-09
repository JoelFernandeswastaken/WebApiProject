using MediatR;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Employee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace Organization.Application.EmployeeModule.Queries.GetTotalCountEmployee
{
    public class GetTotalCountEmployeeQueryHandler : IRequestHandler<GetTotalCountEmployeeQuery, ErrorOr<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTotalCountEmployeeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public async Task<ErrorOr<int>> Handle(GetTotalCountEmployeeQuery request, CancellationToken cancellationToken)
        {
            var employee = new Employee();
            int count = await _unitOfWork.Employees.GetTotalCountAsyc(employee);
            return count;
        }
    }
}
