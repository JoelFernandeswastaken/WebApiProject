using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Errors;

namespace Organization.Application.UserModule.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, ErrorOr<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoginUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetUserByEmail("Email", request.email);
            if (user is not null && BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
                return "JwtToken";
            return Errors.Users.IncorrectEmailOrPassword();
        }
    }
}
