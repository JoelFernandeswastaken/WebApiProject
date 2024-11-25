using Organization.Domain.Common.Models;
using Organization.Domain.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.UserDetails
{
    [TableName("tblUserDetails")]
    public sealed class UserDetails : IDbEntity
    {
        [ColumnName("Id")]
        public string Id { get; set; }
        [ColumnName("Email")]
        public string Email { get; set; }
        [DistinguishingUniqueKey]
        [ColumnName("Username")]
        public string Username { get; set; }
        [ColumnName("PasswordHash")]
        public string PasswordHash { get; set; }
        [ColumnName("IsDeleted")]
        public bool IsDeleted { get; set; }
        [ColumnName("RefreshToken")]
        public string RefreshToken { get; set; }
        [ColumnName("TokenExpiration")]
        public string TokenExpiration { get; set; }  
    }
}
