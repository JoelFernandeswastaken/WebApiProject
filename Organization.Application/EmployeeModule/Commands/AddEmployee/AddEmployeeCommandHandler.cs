using MediatR;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Employee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Commands.AddEmployee
{
    public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            string guid = Guid.NewGuid().ToString().Replace("/", "_").Replace("+", "-").Substring(0, 22);

            _unitOfWork.BeginTransaction();
            var id = await _unitOfWork.Employees.AddAsync(new Employee()
            {
                Id = guid,
                Name = request.Name,
                Position = request.Position,
                CompanyId = request.CompanyID,
                CreatedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                Salary = request.Salary,
                Age = request.Age
            });
            _unitOfWork.CommitAndCloseConnection();

            return id;
        }
    }
}
