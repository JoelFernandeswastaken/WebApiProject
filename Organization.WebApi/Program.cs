using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Organization.Application.Configuration;
using Organization.Infrastructure.Configuration;
using Organization.Presentation.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// register the servies of  all layers

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
            c.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
