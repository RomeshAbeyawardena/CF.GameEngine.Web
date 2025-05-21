using CF.Identity.Api;
using CF.Identity.Api.Endpoints.Connect;
using CF.Identity.Api.Endpoints.Users;
using CF.Identity.Api.Extensions;
using CF.Identity.Infrastructure.SqlServer.Extensions;
using FluentValidation;
using IDFCR.Shared.FluentValidation.Extensions;
using IDFCR.Shared.Http.Extensions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackendDependencies("CFIdentity")
    .AddGenericExceptionHandler()
    .AddMediatR(s => s.RegisterServicesFromAssemblyContaining<Program>()
        .AddRoleRequirementPreProcessor()
        .AddFluentValidationRequestPreProcessor())
    .AddRoleRequirementServices()
    .AddLinkDependencies<Program>()
    .AddValidatorsFromAssemblyContaining<Program>()
    //.AddRateLimiter(opt => opt.AddPolicy("",))
    .AddSingleton<IAuthorizationHandler, ScopeClaimPolicyHandler>()
    .AddSingleton<IAuthorizationPolicyProvider, ScopeClaimPolicyProvider>()
    .AddAuthentication(SystemAuthenticationScheme.Name)
    .AddScheme<AuthenticationSchemeOptions, AuthHandler>(SystemAuthenticationScheme.Name, options => {
    });
    
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = SystemAuthenticationScheme.Name;
    options.DefaultChallengeScheme = SystemAuthenticationScheme.Name;
});
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
//app.UseAuthMiddleware();
app.AddConnectEndpoints();
app.AddUserEndpoints();
//app.UseRateLimiter();

app.Run();
