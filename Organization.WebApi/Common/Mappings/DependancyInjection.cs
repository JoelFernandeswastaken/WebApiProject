using Mapster;
using MapsterMapper;
using System.Reflection;

namespace Organization.Presentation.Api.Common.Mappings
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper> ();  

            return services;
        }
    }
}
