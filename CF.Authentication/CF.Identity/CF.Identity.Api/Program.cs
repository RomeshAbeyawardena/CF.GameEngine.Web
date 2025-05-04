using CF.Identity.Api.Endpoints.Authenticate;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.AddAuthenticationEndpoints();
app.Run();
