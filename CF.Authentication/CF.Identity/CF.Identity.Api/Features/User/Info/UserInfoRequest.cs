using CF.Identity.Api.Features.AccessTokens;
using CF.Identity.Api.Features.AccessTokens.Get;
using CF.Identity.Api.Features.Clients;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.User.Get;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.User.Info;

public record UserInfoRequest(string AccessToken, string ClientId) : IUnitRequest<UserInfoResponse>;

public class UserInfoRequestHandler(IMediator mediator, IClientCredentialHasher clientCredentialHasher, TimeProvider timeProvider) : IUnitRequestHandler<UserInfoRequest, UserInfoResponse>
{
    public async Task<IUnitResult<UserInfoResponse>> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        var clients = await mediator.Send(new FindClientQuery(request.ClientId), cancellationToken);
        var client = clients.GetOneOrDefault();
        if (client is null)
        {
            return UnitResult.NotFound<UserInfoResponse>(request.ClientId).As<UserInfoResponse>();
        }

        var hash = clientCredentialHasher.Hash(request.AccessToken, client);
        var utcNow = timeProvider.GetUtcNow();
        var accessTokens = await mediator.Send(new FindAccessTokenQuery(hash, client.Id, ValidFrom: utcNow, ValidTo: utcNow), cancellationToken);

        var accessToken = accessTokens.GetOneOrDefault(orderedTranform : x => x.OrderByDescending(a => a.ValidFrom));
        if (accessToken is null)
        {
            return new UnitResult(new UnauthorizedAccessException()).As<UserInfoResponse>();
        }

        //TODO get associated user and return response
        var usersResult = await mediator.Send(new FindUserQuery(accessToken!.UserId), cancellationToken);

        if (usersResult.HasValue)
        {
            var user = usersResult.Result;
            return UnitResult.FromResult(new UserInfoResponse(user.Id.ToString(), user.FormatName(), 
                user.PreferredUsername ?? user.Username, user.EmailAddress), UnitAction.Get);
        }
        return new UnitResult(new UnauthorizedAccessException()).As<UserInfoResponse>();
    }
}