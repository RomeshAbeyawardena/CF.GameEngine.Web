using CF.Identity.Api.Endpoints.Connect;
using CF.Identity.Api.Extensions;
using CF.Identity.Infrastructure.SqlServer.Extensions;
using IDFCR.Shared.FluentValidation.Extensions;
using IDFCR.Shared.Http.Extensions;

using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackendDependencies("CFIdentity")
    .AddGenericExceptionHandler()
    .AddMediatR(s => s.RegisterServicesFromAssemblyContaining<Program>()
        .AddRoleRequirementPreProcessor())
    .AddRoleRequirementServices()
    .AddLinkDependencies<Program>()
    //.AddRateLimiter(opt => opt.AddPolicy("",))
    .AddAuthentication("ClientBearer")
    .AddScheme<AuthenticationSchemeOptions, AuthHandler>("ClientBearer", options => {
    });
    
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "ClientBearer";
    options.DefaultChallengeScheme = "ClientBearer";
});
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
//app.UseAuthMiddleware();
app.AddConnectEndpoints();
//app.UseRateLimiter();

app.Run();
