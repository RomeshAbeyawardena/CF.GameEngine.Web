using CF.Identity.Infrastructure.SqlServer.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackendDependencies("CFIdentity");
var app = builder.Build();


app.Run();
