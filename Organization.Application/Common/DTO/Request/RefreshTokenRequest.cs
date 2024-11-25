using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.DTO.Request
{
    public class RefreshTokenRequest
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}
