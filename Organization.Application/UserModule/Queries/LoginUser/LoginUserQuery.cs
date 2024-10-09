using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace Organization.Application.UserModule.Queries.LoginUser
{
    public record class LoginUserQuery(string email, string password) : IRequest<ErrorOr<string>>
    {
    }
}
