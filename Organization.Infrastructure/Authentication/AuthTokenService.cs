using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Organization.Application.Common.ApplicationConfiguration;
using Organization.Application.Common.Interfaces.Authentication;
using Organization.Domain.UserDetails;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Authentication
{
    public class AuthTokenService : IAuthTokenService
    {
        private readonly JwtOptions _options;
        public AuthTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }         
        public async Task<string> GenerateToken(UserDetails user)
        {
            // throw new NotImplementedException();

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret)), SecurityAlgorithms.HmacSha512Signature),
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(securityTokenDescriptor));
        }

    }
}
