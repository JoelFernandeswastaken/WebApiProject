using ErrorOr;
using MediatR;
using Organization.Application.Common.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Organization.Application.UserModule.Commands.RefreshToken
{
    public record RefreshTokenCommand(string accessToken, string refreshToken) :IRequest<ErrorOr<string>>
    {
    }
}
