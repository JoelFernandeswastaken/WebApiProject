using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Common.Utilities;
using Organization.Infrastructure.Persistance;
using Organization.Presentation.Api.Common.Exceptions;
using Organization.Presentation.Api.Swagger;
using Organization.Presentation.Api.Swagger.Examples.Response;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Organization.Presentation.Api.Common.Mappings;
using System.Net;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Organization.Application.Common.Utilities;
using Organization.Application.Common.ApplicationConfiguration;

namespace Organization.Presentation.Api.Configuration
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            var secretKey = new ConfigurationBuilder().AddUserSecrets<Program>().Build().GetSection(GlobalConstants.JWT).GetValue<string>("Secret");

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.DocumentFilter<DisableApiAttribute>();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlPath));
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
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                // options.ApiVersionReader = new QueryStringApiVersionReader("organizationAppTest-api-version"); // query string method for api versioning (here api url remains the same for all versions)
                // options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version"); // for header versioning (version passed in header)
            });

            services.AddAuthentication(x =>
            {
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    // IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("JwZ8x@R1tP$u3QwL6!sE9kB&vF7mJ^nC5#dT0zH*oP2yY!qR4jW1aS+eX8vN3zK0")),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // services.AddSwaggerExamplesFromAssemblyOf<GetCompaniesV2ResponseExample>();

            // configure swagger to work with versioning
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddMappings();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();
            return services;
        }

    }
}
