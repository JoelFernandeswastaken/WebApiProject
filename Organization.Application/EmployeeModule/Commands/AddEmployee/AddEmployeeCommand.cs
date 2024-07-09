using MediatR;
using Organization.Application.Common.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Commands.AddEmployee
{
    public sealed record class AddEmployeeCommand(EmployeeRequest employeeRequest) : IRequest<string>
    {
    }
}
