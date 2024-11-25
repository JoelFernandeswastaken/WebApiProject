using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using Organization.Application.Common.ApplicationConfiguration;
using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Interfaces.Authentication;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.UserDetails;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Authentication
{
    public class AuthTokenService : IAuthTokenService
    {
        private readonly JwtOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public AuthTokenService(IOptions<JwtOptions> options, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _options = options.Value;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> DoTokenCreation(UserDetails user)
        {
            string accessTokenString = GenerateToken(user);
            RefreshToken refreshToken = GenerateRefreshToken();
            SetRefreshTokenAsHttpCookie(refreshToken);

            _unitOfWork.BeginTransaction();
            user.RefreshToken = refreshToken.Token;
            user.TokenExpiration = refreshToken.Expiration.ToString("yyyy-MM-dd hh:mm:ss");
            await _unitOfWork.Users.UpdateAsync(user);
            _unitOfWork.CommitAndCloseConnection();

            return accessTokenString;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken();

            refreshToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            refreshToken.Expiration = DateTime.UtcNow.AddDays(7);

            return refreshToken;
        }

        public string GenerateToken(UserDetails user)
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

        public void SetRefreshTokenAsHttpCookie(RefreshToken refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expiration
            };

            var httpContext = _httpContextAccessor.HttpContext;
            httpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        public JwtSecurityToken ExtractPayloadClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();   
            var payload = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var username = payload.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var email = payload.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            return payload;
        }
    }
}
