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
        public static class Employee
        {
            public static Error EmployeeDoesNotExist(string msg = null) => Error.NotFound(code: "EMP001", description: msg ?? "Employee does not exist.");
            public static Error DuplicateEmployee(string msg = null) => Error.Conflict(code: "EMP002", description: msg ?? "Employee already exists in system.");
        }
    }
}
