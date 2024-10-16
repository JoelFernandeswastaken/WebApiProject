using Organization.Domain.UserDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Interfaces.Authentication
{
    public interface IAuthTokenService
    {
        public Task<string> GenerateToken(UserDetails user);
    }
}
