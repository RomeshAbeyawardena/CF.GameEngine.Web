using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Extensions;
using MediatR;
using System.Text;

namespace CF.Identity.Api.Extensions;

public class ClientSecretMiddleware 
{
    public async static Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var services = context.RequestServices;
        var logger = services.GetRequiredService<ILogger<ClientSecretMiddleware>>();

        try
        {
            
            string[] requiredPaths = ["api", "connect"];
            var path = (context.Request.Path.Value ?? string.Empty).ToLowerInvariant();
            if (!requiredPaths.Any(p => path.StartsWith($"/{p}")))
            {
                await next(context);
            }

            var auth = context.Request.Headers["x-auth"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(auth))
            {
                throw new NullReferenceException("Authentication header missing");
            }

            
            var encoding = services.GetRequiredService<Encoding>();

            var raw = encoding.GetString(Convert.FromBase64String(auth));

            var parts = raw.Split(':', 2);
            if (parts.Length != 2)
            {
                throw new NullReferenceException("Authentication header invalid");
            }

            var clientId = parts[0];
            var clientSecret = parts[1];

            if (string.IsNullOrWhiteSpace(clientId)
                 || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new NullReferenceException("Authentication header invalid, two-part requirement is incorrect");
            }

            var timeProvider = services.GetRequiredService<TimeProvider>();
            var utcNow = timeProvider.GetUtcNow();

            var range = DateTimeOffsetRange.GetValidatyDateRange(utcNow);
            var mediator = services.GetRequiredService<IMediator>();
            var clientResult = (await mediator.Send(new FindClientQuery(clientId, range.FromValue, range.ToValue, Bypass: true))).GetOneOrDefault();

            var clientProtection = services.GetRequiredService<IClientProtection>();
            if (clientResult is null
                || !clientProtection.VerifySecret(clientResult, clientSecret))
            {
                throw new UnauthorizedAccessException("Client authentication failed");
            }

            context.Items.Add(nameof(AuthenticatedClient), new AuthenticatedClient(clientId, clientResult));
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authentication failed");
            logger.LogError(ex, "Client secret authentication failed: {Message}", ex.Message);
            return;
        }
    }
}
