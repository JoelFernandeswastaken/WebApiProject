using MediatR;
using Organization.Application.Common.DTO.Request;
using Organization.Domain.Employee;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Commands.UpdateEmployee
{
    public sealed record class UpdateEmployeeCommand(string? id, int Age, string? Name, string? Position, string? CompanyID, int Salary) : IRequest<bool>;
}
