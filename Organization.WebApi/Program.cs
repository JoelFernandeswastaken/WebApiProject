using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Organization.Application.Common.Utilities;
using Organization.Application.Configuration;
using Organization.Infrastructure.Configuration;
using Organization.Presentation.Api.Configuration;
using Serilog;


var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); 
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).Enrich.FromLogContext().CreateLogger();

try
{
    Log.Information("{ApplicationName} has Started.", GlobalConstants.ApplicationName);
    // Add services to the container.

    // register the servies of  all layers
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddApplication()
        .AddPresentation()
        .AddInfrastructure();

    var app = builder.Build();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        // app.UseSwaggerUI(s => s.SwaggerEndpoint("v1/swagger.json", "Organization API V1"));
        app.UseSwaggerUI(c =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                c.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                // c.RoutePrefix = "api/documentation";
                c.DisplayRequestDuration(); // show response times
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List); // default is list
                c.DefaultModelExpandDepth(1);
            }
        });
    }

    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal("{ApplicationName} has failed to start.", GlobalConstants.ApplicationName);
}
finally
{
    Log.CloseAndFlush();
}