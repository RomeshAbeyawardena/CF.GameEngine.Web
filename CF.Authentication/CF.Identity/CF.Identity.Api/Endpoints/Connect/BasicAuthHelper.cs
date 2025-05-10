using CF.Identity.Api.Features.Client.Get;
using CF.Identity.Infrastructure.Features.Clients;
using MediatR;
using System.Text;

namespace CF.Identity.Api.Endpoints.Connect;

public static class BasicAuthHelper
{
    public static Task<(bool success, IClient? client)> TryAuthenticateAsync(
        this IHttpContextAccessor contextAccessor,
        CancellationToken cancellationToken = default)
    {
        var context = contextAccessor.HttpContext ?? throw new NullReferenceException("HttpContext not available in this context");
        var services = context.RequestServices;
        return TryAuthenticateAsync(contextAccessor,
            services.GetRequiredService<IClientCredentialHasher>(),
            services.GetRequiredService<IMediator>(),
            cancellationToken);
    }

    public static async Task<(bool success, IClient? client)> TryAuthenticateAsync(
        IHttpContextAccessor contextAccessor,
        IClientCredentialHasher clientCredentialHasher,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var context = contextAccessor.HttpContext ?? throw new NullReferenceException("HttpContext not available in this context");

        var header = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(header) || !header.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            return (false, null);

        string raw;
        try
        {
            raw = Encoding.UTF8.GetString(Convert.FromBase64String(header["Basic ".Length..]));
        }
        catch
        {
            return (false, null);
        }

        var parts = raw.Split(':', 2);
        if (parts.Length != 2)
            return (false, null);

        var clientId = parts[0];
        var clientSecret = parts[1];

        var result = await mediator.Send(new FindClientQuery(clientId), cancellationToken);
        var client = result.Result?.FirstOrDefault();

        if (client is null)
        {
            return (false, null);
        }
        // Assuming your hasher is available via DI or passed in too
        if (!clientCredentialHasher.Verify(clientSecret, client))
        {
            return (false, null);
        }

        return (true, client);
    }
}

