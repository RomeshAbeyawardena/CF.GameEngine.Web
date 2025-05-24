using CF.Identity.Api;
using CF.Identity.Api.Endpoints;
using CF.Identity.Api.Extensions;
using CF.Identity.Api.Features;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.SqlServer.Extensions;
using FluentValidation;
using IDFCR.Http.Authentication.Extensions;
using IDFCR.Shared.FluentValidation.Extensions;
using IDFCR.Shared.Http.Extensions;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackendDependencies("CFIdentity")
    .AddGenericExceptionHandler()
    .AddMediatR(s => s.RegisterServicesFromAssemblyContaining<Program>()
        .AddRoleRequirementPreProcessor()
        .AddFluentValidationRequestPreProcessor())
    .AddRoleRequirementServices()
    .AddLinkDependencies<Program>()
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.EnableAnnotations(); // If using [SwaggerOperation] etc.
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Identity API",
            Version = "v1",
            Description = "Identity API"
        });

        options
            .UseRuntimeServer()
            .UseOpenApiVersionFromConfig()
            .UseOpenApiContactDocumentFilter();
    })
    //.AddRateLimiter(opt => opt.AddPolicy("",))
    .AddScopeBasedAuthorization(SystemAuthenticationScheme.Name, SystemRoles.GlobalRead)
    .AddScheme<AuthenticationSchemeOptions, AuthHandler>(SystemAuthenticationScheme.Name, (opt) => { });

builder.Services.AddAuthorization();


var app = builder.Build();

app
    .UseAuthentication()
    .UseAuthorization();

app.AddEndpoints();
//app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", async (HttpContext) =>
    {
        await Task.CompletedTask;
        HttpContext.Response.Redirect("swagger");
    }).AllowAnonymous();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GameEngine API v1");
    });
}

app.Run();
