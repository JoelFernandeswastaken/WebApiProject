using MediatR;
using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace Organization.Application.EmployeeModule.Queries.GetEmployeeByID
{
    public class GetEmployeeByIDQueryHandler : IRequestHandler<GetEmployeeByIDQuery, ErrorOr<EmployeeResponseV2>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEmployeeByIDQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<EmployeeResponseV2>> Handle(GetEmployeeByIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetByIdAsync(request.id);
            if(result == null)
                return Errors.Employee.EmployeeDoesNotExist($"Company with given ID {request.id} does not exist");
            var employeeResponse = new EmployeeResponseV2(result.Name, result.Age, result.Position, Convert.ToInt32(result.Salary), result.CreatedOn);
            //var employeeResponse = new EmployeeResponse()
            //{
            //    Name = result.Name,
            //    Age = result.Age,
            //    Position = result.Position,
            //    Salary = Convert.ToInt32(result.Salary),
            //    CreatedOn = result.CreatedOn
            //};
            return employeeResponse;
        }
    }
}
