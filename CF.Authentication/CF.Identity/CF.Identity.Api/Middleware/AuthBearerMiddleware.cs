using CF.Identity.Api.Extensions;
using CF.Identity.Api.Features.AccessTokens.Get;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Extensions;
using MediatR;

namespace CF.Identity.Api.Middleware;

public record AuthenticatedClient(string ClientId, IClientDetails ClientDetails);

public static partial class AuthMiddleware
{
    public const string ClientItemKey = "Client";
    public const string TokenItemKey = "AccessToken";

    public static IApplicationBuilder UseAuthBearerMiddleware(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            bool @continue = true;
            try
            {
                var authorisation = context.Request.Headers.Authorization.FirstOrDefault();

                var client = context.GetAuthenticatedClient();

                if (client is null || string.IsNullOrWhiteSpace(authorisation)
                || !authorisation.StartsWith("Bearer", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                var accessToken = authorisation["Bearer ".Length..].Trim();
                
                var clientCredentialHasher = context.RequestServices.GetRequiredService<IClientCredentialHasher>();

                var hash = clientCredentialHasher.Hash(accessToken, client.ClientDetails.Map<ClientDto>());

                var timeProvider = context.RequestServices.GetRequiredService<TimeProvider>();
                var utcNow = timeProvider.GetUtcNow();

                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                var accessTokens = await mediator.Send(new FindAccessTokenQuery(hash, client.ClientDetails.Id,
                    ValidFrom: utcNow.Date.AddDays(1).AddHours(-1),
                    ValidTo: utcNow.Date));

                var validAccessToken = accessTokens.GetOneOrDefault(orderedTranform: x => x.OrderByDescending(a => a.ValidFrom));
                if (validAccessToken is null)
                {
                    @continue = false;
                    return;
                }

                context.Items[TokenItemKey] = validAccessToken;
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
