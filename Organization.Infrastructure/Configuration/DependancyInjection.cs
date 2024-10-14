using Microsoft.Extensions.DependencyInjection;
using Organization.Application.Common.ApplicationConfiguration;
using Organization.Application.Common.Interfaces.Authentication;
using Organization.Infrastructure.Authentication;
using Organization.Infrastructure.Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Configuration
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<DapperDataContext>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.ConfigureOptions<JwtOptionsSetup>();
            return services;
        }
    }
}
