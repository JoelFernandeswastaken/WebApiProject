using MediatR;
using Organization.Domain.UserDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Errors;

namespace Organization.Application.UserModule.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ErrorOr<UserDetails>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserByEmailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<UserDetails>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Users.GetUserByEmail("Email", request.email);
            if (result == null)
                return Errors.Users.UserNotFound();
            return new UserDetails()
            {
                Id = result.Id,
                Email = result.Email,
                Username = result.Username,
                PasswordHash = result.PasswordHash,
            };
        }
    }
}
