using MediatR;
using Organization.Application.Common.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace Organization.Application.EmployeeModule.Commands.AddEmployee
{
    public sealed record class AddEmployeeCommand(int Age, string Name, string Position, string CompanyID, int Salary) : IRequest<ErrorOr<string>>
    {
    }
}
