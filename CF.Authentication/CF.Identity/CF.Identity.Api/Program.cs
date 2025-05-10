using CF.Identity.Infrastructure.SqlServer.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackendDependencies("");
var app = builder.Build();


app.Run();
