using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Extensions;
using MediatR;
using System.Text;

namespace CF.Identity.Api.Middleware;

public record AuthenticatedClient(string ClientId, IClientDetails ClientDetails);

public static class AuthMiddleware
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            bool @continue = true;
            try
            {
                var auth = context.Request.Headers["x-auth"].FirstOrDefault();

                if (string.IsNullOrWhiteSpace(auth))
                {
                    // No client header provided — let other middleware handle access control
                    return;
                }

                var raw = Encoding.UTF8.GetString(Convert.FromBase64String(auth));

                var parts = raw.Split(':', 2);
                if (parts.Length != 2)
                {
                    @continue = false;
                    return;
                }

                var clientId = parts[0];
                var clientSecret = parts[1];

                if (string.IsNullOrWhiteSpace(clientId) 
                     || string.IsNullOrWhiteSpace(clientSecret))
                {
                    @continue = false;
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var mediator = context.RequestServices.GetRequiredService<IMediator>();

                var clientCredentialHasher = context.RequestServices.GetRequiredService<IClientCredentialHasher>();

                var clientResult = (await mediator.Send(new FindClientQuery(clientId))).GetOneOrDefault();

                if (clientResult is null
                    || !clientCredentialHasher.Verify(clientSecret, clientResult))
                {
                    @continue = false;
                    return;
                }

                context.Items["Client"] = new AuthenticatedClient(clientId,
                    clientResult); 
            }
            finally
            {
                if (@continue)
                {
                    await next.Invoke();
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
            }
        });
    }
}
