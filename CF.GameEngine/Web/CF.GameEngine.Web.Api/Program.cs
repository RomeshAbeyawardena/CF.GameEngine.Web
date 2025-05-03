using CF.GameEngine.Infrastructure.SqlServer.Extensions;
using CF.GameEngine.Web.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
    builder.Services
    .AddSingleton(TimeProvider.System)
    .AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>())
    .AddBackendDependencies("GameEngineDb");

var app = builder.Build();

app.AddApiEndpoints();

app.Run();
