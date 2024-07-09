using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EmployeeModule.Commands.DeleteEmployee
{
    public sealed record class DeleteEmployeeCommand(string id) : IRequest<int>;
}
