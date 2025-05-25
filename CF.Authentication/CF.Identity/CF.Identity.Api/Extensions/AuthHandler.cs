using CF.Identity.Api.Features.AccessTokens.Get;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.Scopes.Get;
using CF.Identity.Api.Features.User.Get;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace CF.Identity.Api.Extensions;

public class AuthHandler(Encoding encoding, IMediator mediator, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) 
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private AuthenticatedClient? authenticatedClient;

    private async Task AttachScopes(IClientDetails client, Guid userId, List<Claim> claims)
    {
        var scopes = (await mediator.Send(new FindScopeQuery(client.Id, userId, IncludePrivilegedScoped: client.IsSystem, Bypass: true))).GetResultOrDefault();

        if(scopes is null || !scopes.Any())
        {
            return;
        }

        foreach(var scope in scopes)
        {
            claims.Add(new(ClaimTypes.Role, scope.Key));
        }
        
    }

    protected async Task<AuthenticateResult?> ExtractAndValidateClientHeaderAsync()
    {        
        var auth = Context.Request.Headers["x-auth"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(auth))
        {
            return AuthenticateResult.Fail("Authentication header missing");
        }

        var raw = encoding.GetString(Convert.FromBase64String(auth));

        var parts = raw.Split(':', 2);
        if (parts.Length != 2)
        {
            return AuthenticateResult.Fail("Authentication header invalid");
        }

        var clientId = parts[0];
        var clientSecret = parts[1];

        if (string.IsNullOrWhiteSpace(clientId)
             || string.IsNullOrWhiteSpace(clientSecret))
        {
            return AuthenticateResult.Fail("Authentication header invalid, two-part requirement is incorrect");
        }

        var clientCredentialHasher = Context.RequestServices.GetRequiredService<IClientCredentialHasher>();

        var timeProvider = Context.RequestServices.GetRequiredService<TimeProvider>();
        var utcNow = timeProvider.GetUtcNow();

        var range = DateTimeOffsetRange.GetValidatyDateRange(utcNow);

        var clientResult = (await mediator.Send(new FindClientQuery(clientId, range.FromValue, range.ToValue, Bypass: true))).GetOneOrDefault();

        if (clientResult is null
            || !clientCredentialHasher.Verify(clientSecret, clientResult))
        {
            return AuthenticateResult.Fail("Client authentication failed");
        }

        authenticatedClient = new(clientId,
                    clientResult);

        return null;
    }

    private async Task<AuthenticateResult> ExtractAndValidateBearingTokenAsync()
    {
        var authorisation = Context.Request.Headers.Authorization.FirstOrDefault();

        var client = authenticatedClient;

        if (client is null || string.IsNullOrWhiteSpace(authorisation)
        || !authorisation.StartsWith("Bearer", StringComparison.InvariantCultureIgnoreCase))
        {
            return AuthenticateResult.Fail("Invalid authentication scheme");
        }

        var accessToken = authorisation["Bearer ".Length..].Trim();

        var clientCredentialHasher = Context.RequestServices.GetRequiredService<IClientCredentialHasher>();

        var hash = clientCredentialHasher.Hash(accessToken, client.ClientDetails.Map<ClientDto>());

        var timeProvider = Context.RequestServices.GetRequiredService<TimeProvider>();
        var utcNow = timeProvider.GetUtcNow();

        var range = DateTimeOffsetRange.GetValidatyDateRange(utcNow);

        var accessTokens = await mediator.Send(new FindAccessTokenQuery(hash, client.ClientDetails.Id,
            ValidFrom: range.FromValue,
            ValidTo: range.ToValue, Bypass: true));

        var validAccessToken = accessTokens.GetOneOrDefault(orderedTranform: x => x.OrderByDescending(a => a.ValidFrom));
        if (validAccessToken is null)
        {
            return AuthenticateResult.Fail("Access token failed");
        }

        const string Bearer = "Bearer";
        var claims = new List<Claim>
        {
            new(ClaimTypes.GroupSid, client.ClientDetails.Id.ToString()),
            new(ClaimTypes.Sid, client.ClientDetails.Name),
            new(ClaimTypes.System, client.ClientDetails.IsSystem.ToString()),
            new(ClaimTypes.Authentication, accessToken)
        };

        var user = (await mediator.Send(new GetUserByIdQuery(validAccessToken.UserId, Bypass: true))).GetResultOrDefault();

        if(user is null)
        {
            return AuthenticateResult.Fail("User not found");
        }

        claims.Add(new(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claims.Add(new(ClaimTypes.Name, user.PreferredUsername ?? user.Username));
        claims.Add(new(ClaimTypes.Email, user.EmailAddress));
        claims.Add(new(ClaimTypes.GivenName, user.FormatName()));

        await AttachScopes(client.ClientDetails, user.Id, claims);

        var identity = new ClaimsIdentity(claims, Bearer);
        var principal = new ClaimsPrincipal(identity);

        return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var result = await ExtractAndValidateClientHeaderAsync();
        if(result is not null)
        {
            return result;
        }

        return await ExtractAndValidateBearingTokenAsync();
    }
}
