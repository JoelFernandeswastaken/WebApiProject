using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace Organization.Application.UserModule.Commands.RegisterUser
{
    public record RegisterUserCommand(string username, string email, string password) : IRequest<ErrorOr<string>>;
}
