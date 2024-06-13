using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Utilities;
using Organization.Infrastructure.Persistance;
using Organization.Presentation.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Presentation.Api.Configuration
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => {
                c.DocumentFilter<DisableApiAttribute>();
                //c.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Version = "v1",
                //    Title = "Organization API",
                //    Description = "An ASP.NET Core Web API for managing an organzation",
                //    TermsOfService = new Uri("https://example.com/terms"),
                //    Contact = new OpenApiContact
                //    {
                //        Name = "Example Contact",
                //        Url = new Uri("https://example.com/contact")
                //    },
                //    License = new OpenApiLicense
                //    {
                //        Name = "Example License",
                //        Url = new Uri("https://example.com/license")
                //    }
                //});
            });
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                // options.ApiVersionReader = new QueryStringApiVersionReader("organizationAppTest-api-version"); // query string method for api versioning (here api url remains the same for all versions)
                options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version"); // for header versioning (version passed in header)
            });

            // configure swagger to work with versioning
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

    }
}
