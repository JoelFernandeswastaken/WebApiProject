using Azure.Core;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Organization.Application.Common.PipelineBehaviours;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Organization.Application.Configuration
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSerilog();
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(typeof(DependancyInjection).Assembly);
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>));
            services.AddValidatorsFromAssembly(typeof(DependancyInjection).Assembly);

            return services;
        }
    }
}
