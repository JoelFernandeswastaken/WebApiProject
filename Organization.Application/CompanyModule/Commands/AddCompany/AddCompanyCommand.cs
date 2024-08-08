using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.CompanyModule.Commands.AddCompany
{
    public sealed record class AddCompanyCommand(string Name, string Address, string Country) : IRequest<ErrorOr<string>>;
}
