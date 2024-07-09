using MediatR;
using Organization.Application.Common.Exceptions;
using Organization.Application.Common.Interfaces.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeToDelete = await _unitOfWork.Employees.GetByIdAsync(request.id);
            if (employeeToDelete == null)
                throw new EmployeeNotFoundException($"Could not find employee with given ID: {request.id}");

            _unitOfWork.BeginTransaction();
            int rowsAffected = await _unitOfWork.Employees.SoftDeleteAsync(request.id);
            _unitOfWork.CommitAndCloseConnection();

            return rowsAffected;
        }
    }
}
