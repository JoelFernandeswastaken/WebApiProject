using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Errors;
using Organization.Application.Common.Interfaces.Authentication;

namespace Organization.Application.UserModule.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, ErrorOr<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthTokenService _jwtTokenGenerator;
        public LoginUserQueryHandler(IUnitOfWork unitOfWork, IAuthTokenService jwtTokenGenertor)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenertor;  
        }
        public async Task<ErrorOr<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetUserByEmail("Email", request.email);
            if (user is not null && BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
                // return "Token";
                return await _jwtTokenGenerator.GenerateToken(user);
            return Errors.Users.IncorrectEmailOrPassword();
        }
    }
}
