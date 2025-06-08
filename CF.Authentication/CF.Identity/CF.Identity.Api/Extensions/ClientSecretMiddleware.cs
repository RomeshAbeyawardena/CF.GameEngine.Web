using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using System.Text;

namespace CF.Identity.Api.Extensions;

public class ClientSecretException(string message, Exception? innerException = null, string? exposableMessage = null, string? details = null)
    : Exception(message, innerException), IExposableException
{
    string IExposableException.Message => exposableMessage ?? Message;
    string? IExposableException.Details => details;
}

public class ClientSecretMiddleware
{
    private static async Task AuthenticationFailed(Exception exception, HttpContext context, ILogger logger)
    {
        var message = "Authentication failed";

        if (exception is IExposableException exposableException)
        {
            message = exposableException.Message;
        }

        var result = UnitResult.Failed<object>(new Exception(message, exception), FailureReason: FailureReason.Unauthorized);

        await context.Response.WriteAsJsonAsync(result.ToApiResult());
        logger.LogError(exception, "Client secret authentication failed: {Message}", exception.Message);
    }

    public async static Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        const string exposableMessage = "Client authentication failed. Please check your client ID and secret header parameters.";

        var services = context.RequestServices;
        var logger = services.GetRequiredService<ILogger<ClientSecretMiddleware>>();

        try
        {

            string[] requiredPaths = ["api", "connect"];
            var path = (context.Request.Path.Value ?? string.Empty).ToLowerInvariant();
            if (!requiredPaths.Any(p => path.StartsWith($"/{p}")))
            {
                await next(context);
                return;
            }

            var auth = context.Request.Headers["x-api-key"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(auth))
            {
                throw new ClientSecretException("Authentication header missing", exposableMessage: exposableMessage);
            }

            var encoding = services.GetRequiredService<Encoding>();

            var raw = encoding.GetString(Convert.FromBase64String(auth));

            var parts = raw.Split(':', 2);
            if (parts.Length != 2)
            {
                throw new ClientSecretException("Authentication header invalid", exposableMessage: exposableMessage);
            }

            var clientId = parts[0];
            var clientSecret = parts[1];

            if (string.IsNullOrWhiteSpace(clientId)
                 || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ClientSecretException("Authentication header invalid, two-part requirement is incorrect", exposableMessage: exposableMessage);
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
                throw new ClientSecretException("Client authentication failed", exposableMessage: exposableMessage);
            }

            context.Items.Add(nameof(AuthenticatedClient), new AuthenticatedClient(clientId, clientResult));
            await next(context);
        }
        catch (FormatException ex)
        {
            await AuthenticationFailed(ex, context, logger);
        }
        catch (ClientSecretException ex)
        {
            await AuthenticationFailed(ex, context, logger);
        }
        catch (UnauthorizedAccessException ex)
        {
            await AuthenticationFailed(ex, context, logger);
        }
    }
}
