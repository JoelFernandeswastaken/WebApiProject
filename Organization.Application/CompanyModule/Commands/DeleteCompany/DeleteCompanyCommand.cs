using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace Organization.Application.CompanyModule.Commands.DeleteCompany
{
    public record class DeleteCompanyCommand(string id, bool deleteAssociations) : IRequest<ErrorOr<int>>;
}
