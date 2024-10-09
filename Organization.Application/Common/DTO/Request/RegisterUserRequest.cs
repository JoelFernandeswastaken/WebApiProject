using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.DTO.Request
{
    public class RegisterUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }   
    }
}
