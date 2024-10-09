using Organization.Application.Common.DTO.Request;
using Organization.Domain.UserDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Interfaces.Persistance
{
    public interface IUserRepository : IGenericRepository<UserDetails>
    {
        Task<UserDetails> GetUserByEmail(string emailColumnName, string emailColumnValue);
    }
}
