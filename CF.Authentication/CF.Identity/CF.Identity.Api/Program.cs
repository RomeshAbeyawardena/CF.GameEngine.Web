using CF.Identity.Api.Endpoints.Connect;
using CF.Identity.Infrastructure.SqlServer.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackendDependencies("CFIdentity")
    .AddMediatR(s => s.RegisterServicesFromAssemblyContaining<Program>())
    //.AddRateLimiter(opt => opt.AddPolicy("",))
    .AddHttpContextAccessor();

var app = builder.Build();
app.AddConnectEndpoints();
//app.UseRateLimiter();

app.Run();
