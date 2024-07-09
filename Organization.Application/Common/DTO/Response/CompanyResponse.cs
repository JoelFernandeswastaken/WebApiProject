using Organization.Domain.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.DTO.Response
{
    public sealed record class CompanyResponse(string? Id, string? Name, string? Address, string? Country);
}

