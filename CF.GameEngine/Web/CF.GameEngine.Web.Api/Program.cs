using CF.GameEngine.Infrastructure.SqlServer.Extensions;
using CF.GameEngine.Web.Api.Endpoints;
using FluentValidation;
using IDFCR.Shared.FluentValidation.Extensions;
using IDFCR.Shared.Http.Extensions;
using IDFCR.Shared.Http.Middleware;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services
    .AddSingleton(TimeProvider.System)
    .AddGenericExceptionHandler()
    .AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>()
        .AddFluentValidationRequestPreProcessor())
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddLinkDependencies<Program>()
    .AddBackendDependencies("GameEngineDb")
    .AddEndpointsApiExplorer()
    .AddHttpContextAccessor();

services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); // If using [SwaggerOperation] etc.
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GameEngine API",
        Version = "v1",
        Description = "GameEngine API"
    });

    options
        .UseRuntimeServer()
        .UseOpenApiVersionFromConfig()
        .UseOpenApiContactDocumentFilter();
});

var app = builder.Build();
app.UseStaticFiles();
app.AddApiEndpoints();
app.UseMiddleware<ExceptionMiddleware>();

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
        options.InjectStylesheet("/swagger-ui/swagger-dark.css");
    });
}


app.Run();
