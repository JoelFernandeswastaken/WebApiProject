using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Organization.Application.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Organization.Application.Common.ApplicationConfiguration
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration;
        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(JwtOptions options)
        {
            _configuration.GetSection(GlobalConstants.JWT).Bind(options);
        }
    }
}
