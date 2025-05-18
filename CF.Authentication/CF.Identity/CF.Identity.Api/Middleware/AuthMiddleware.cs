using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Extensions;
using MediatR;
using System.Runtime.CompilerServices;
using System.Text;

namespace CF.Identity.Api.Middleware;

public partial class AuthMiddleware
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder app)
    {
        return app.UseXAuthMiddleware()
            .UseAuthBearerMiddleware();
    }

    public static IApplicationBuilder UseXAuthMiddleware(this IApplicationBuilder app)
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

                var timeProvider = context.RequestServices.GetRequiredService<TimeProvider>();
                var utcNow = timeProvider.GetUtcNow();

                var range = DateTimeOffsetRange.GetValidatyDateRange(utcNow);

                var clientResult = (await mediator.Send(new FindClientQuery(clientId, range.FromValue, range.ToValue))).GetOneOrDefault();

                if (clientResult is null
                    || !clientCredentialHasher.Verify(clientSecret, clientResult))
                {
                    @continue = false;
                    return;
                }

                context.Items[ClientItemKey] = new AuthenticatedClient(clientId,
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
