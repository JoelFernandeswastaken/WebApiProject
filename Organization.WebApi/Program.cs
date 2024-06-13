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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s => s.SwaggerEndpoint("v1/swagger.json", "Organization API V1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
