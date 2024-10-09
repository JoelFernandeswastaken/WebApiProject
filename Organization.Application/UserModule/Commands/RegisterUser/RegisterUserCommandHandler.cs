using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Errors;
using MapsterMapper;

namespace Organization.Application.UserModule.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            bool isUserExists = await _unitOfWork.Users.IsExistingAsync(request.username);
            if (isUserExists)
            {
                return Errors.Users.DuplicateUser();
            }

            string guid = Guid.NewGuid().ToString().Replace("/", "_").Replace("+", "-").Substring(0, 22);

            _unitOfWork.BeginTransaction();
            string id = await _unitOfWork.Users.AddAsync(new Domain.UserDetails.UserDetails()
            {
                Id = guid,
                Email = request.email,
                Username = request.username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.password),
            });
            _unitOfWork.CommitAndCloseConnection();
            return id;

        }
    }
}
