using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Organization.Application.Common.Interfaces.Authentication;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.UserModule.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<string>>
    {
        private readonly IAuthTokenService _authTokenService;   
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        public RefreshTokenCommandHandler(IAuthTokenService authTokenService, IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _authTokenService = authTokenService;
            _unitOfWork = unitOfWork;   
            _httpContext = httpContext;
        }
        public async Task<ErrorOr<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var payload = _authTokenService.ExtractPayloadClaims(request.accessToken);
            string refreshTokenFromCookie = _httpContext.HttpContext.Request.Cookies["refreshToken"];
            var accessToken = string.Empty;
            if(payload != null) 
            {
                string email = payload.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
                var userDetails = await _unitOfWork.Users.GetUserByEmail("Email", email);
                if(userDetails != null)
                {
                   if(userDetails.RefreshToken == null || !refreshTokenFromCookie.Equals(userDetails.RefreshToken))
                        return Errors.Users.InvalidRefreshToken();

                   if(Convert.ToDateTime(userDetails.TokenExpiration) > DateTime.UtcNow)
                        return Errors.Users.ExpiredRefreshToken();

                    accessToken = await _authTokenService.DoTokenCreation(userDetails);
                    return accessToken;
                }
                return Errors.Users.UserNotFound();
            }
            return Errors.UnexpectedErrors.InteralServerError();
        }
    }
}
