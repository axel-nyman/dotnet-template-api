using Scalar.AspNetCore;

using TemplateApi.Application.Endpoints;
using TemplateApi.Domain.Registrations;
using TemplateApi.Infrastructure.Data.Registrations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDomainRegistrations();
builder.Services.AddDataRegistrations(builder.Configuration);
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// TODO: Add your mappings here
app.MapProductEndpoints();

app.Run();