using CF.Identity.Api;
using CF.Identity.Api.Endpoints;
using CF.Identity.Api.Extensions;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.SqlServer.Extensions;
using FluentValidation;
using IDFCR.Http.Authentication.Extensions;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.FluentValidation.Extensions;
using IDFCR.Shared.Http.Extensions;
using IDFCR.Shared.Http.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

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

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            BearerFormat = "Bearer {0}",
            Scheme = "Bearer",
        });

        // X-API-KEY header
        options.AddSecurityDefinition("XApiKey", new OpenApiSecurityScheme
        {
            Description = "Client API Key needed to determine trust. Example: \"X-API-KEY: {key}\"",
            Name = "x-api-key",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "ApiKeyScheme"
        });

        // Apply both globally
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "XApiKey"
                    }
                },
                Array.Empty<string>()
            },
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    }
                },
                Array.Empty<string>()
            }
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

RoleRegistrar.RegisterGlobal<SystemRoles>();

var app = builder.Build();

app
    .UseMiddleware<ExceptionMiddleware>()
    .Use(ClientSecretMiddleware.InvokeAsync)
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
