using Organization.Domain.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Company
{
    public sealed class CompanyQueryParameters : QueryParameters
    {
        public string? CompanyName { get; set; }
    }
}
