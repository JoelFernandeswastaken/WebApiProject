using Microsoft.OpenApi.Validations;
using Organization.Application.Common.DTO.Request;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.UserDetails;
using Organization.Infrastructure.Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public class UserRepository : GenericRepository<UserDetails>, IUserRepository
    {
        public UserRepository(DapperDataContext dapperContext) : base(dapperContext) { }

        public async Task<UserDetails> GetUserByEmail(string emailColumnName, string emailColumnValue)
        {
            var result = await GetByColumnValueAsync(emailColumnName, emailColumnValue);
            return result?.FirstOrDefault();
        }
    }
}
