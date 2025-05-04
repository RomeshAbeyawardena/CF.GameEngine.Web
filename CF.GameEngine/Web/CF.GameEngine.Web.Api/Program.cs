using CF.GameEngine.Infrastructure.SqlServer.Extensions;
using CF.GameEngine.Web.Api.Endpoints;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services
    .AddSingleton(TimeProvider.System)
    .AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>())
    .AddBackendDependencies("GameEngineDb")
    .AddEndpointsApiExplorer();

services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); // If using [SwaggerOperation] etc.
});

var app = builder.Build();

app.AddApiEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", async (HttpContext) =>
    {
        await Task.CompletedTask;
        HttpContext.Response.Redirect("/swagger");
    });
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GameEngine API v1");
    });
}

app.Run();
