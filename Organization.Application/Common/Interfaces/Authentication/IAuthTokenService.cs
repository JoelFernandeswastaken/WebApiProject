using Organization.Application.Common.DTO.Response;
using Organization.Domain.UserDetails;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Interfaces.Authentication
{
    public interface IAuthTokenService
    {
        public string GenerateToken(UserDetails user);
        public RefreshToken GenerateRefreshToken();
        public void SetRefreshTokenAsHttpCookie(RefreshToken refreshToken);
        public Task<string> DoTokenCreation(UserDetails user);
        public JwtSecurityToken ExtractPayloadClaims(string token);
    }
}
