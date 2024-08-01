using MediatR;
using Organization.Application.Common.DTO.Request;
using Organization.Application.Common.Exceptions;
using Organization.Application.Common.Interfaces.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }

        public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var requiredEmployee = await _unitOfWork.Employees.GetByIdAsync(request.id);

            if (requiredEmployee == null || requiredEmployee.Id != request.id)
                throw new EmployeeNotFoundException($"Could not find employee with given ID: {request.id}");
            else
            {
                requiredEmployee.Name = request.Name;
                requiredEmployee.Age = request.Age;
                requiredEmployee.CompanyId = request.CompanyID;
                requiredEmployee.Salary = request.Salary;
                requiredEmployee.Position = request.Position;
                requiredEmployee.ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                _unitOfWork.BeginTransaction();
                var result = await _unitOfWork.Employees.UpdateAsync(requiredEmployee);
                _unitOfWork.CommitAndCloseConnection();

                return result;
            }
        }
    }
}
