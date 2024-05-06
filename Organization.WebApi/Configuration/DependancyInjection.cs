using Organization.Application.Common.Interfaces.Persistance;
using Organization.Infrastructure.Persistance;

namespace Organization.Presentation.Api.Configuration
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

    }
}
