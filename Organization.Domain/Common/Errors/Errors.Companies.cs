using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Company
        {
            public static Error CompanyDoesNotExist(string msg = null) => Error.NotFound(code: "CMP001", description: msg ?? "Company does not exist.");
            public static Error DuplicateCompany(string msg = null) => Error.Conflict(code: "CMP002", description: msg ?? "Company already exists in system.");
        }
    }
}
