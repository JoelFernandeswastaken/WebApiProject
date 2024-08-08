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
        public static class UnexpectedErrors
        {
            public static Error InteralServerError(string msg = null) => Error.NotFound(code: "UNEXPECTEDERR", description: msg ?? "Something went wrong.");
        }
    }
}
