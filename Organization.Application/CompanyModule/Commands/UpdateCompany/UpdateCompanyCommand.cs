using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.CompanyModule.Commands.UpdateCompany
{
    public sealed record class UpdateCompanyCommand(string? Id, string? Name, string? Address, string? Country) : IRequest<bool>;
}
