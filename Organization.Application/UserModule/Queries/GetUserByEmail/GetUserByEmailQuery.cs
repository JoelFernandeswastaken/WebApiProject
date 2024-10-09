using MediatR;
using Organization.Domain.UserDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace Organization.Application.UserModule.Queries.GetUserByEmail
{
    public record GetUserByEmailQuery(string email) : IRequest<ErrorOr<UserDetails>>
    {
    }
}
