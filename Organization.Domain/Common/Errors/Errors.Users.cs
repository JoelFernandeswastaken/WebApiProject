using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Common.Errors
{
    public partial class Errors
    {
        public static class Users
        {
            public static Error DuplicateUser(string msg = null) => Error.Conflict(code: "USR001", description: msg ?? "User already exists in system");
            public static Error UserNotFound(string msg = null) => Error.NotFound(code: "USR002", description: msg ?? "User does not exist");
            public static Error IncorrectEmailOrPassword(string msg = null) => Error.Unauthorized(code: "USR003", description: msg ?? "Incorrect Email or Password");
            public static Error InvalidRefreshToken(string msg = null) => Error.Unauthorized(code: "USR004", description: msg ?? "Invalid refresh token");
            public static Error ExpiredRefreshToken(string msg = null) => Error.Unauthorized(code: "USR005", description: msg ?? "Refresh token Expired");
        }
    }
}
