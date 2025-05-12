using CF.Identity.Api.Features.AccessTokens;
using CF.Identity.Api.Features.AccessTokens.Get;
using CF.Identity.Api.Features.Clients;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.User.Get;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.User.Info;

public record UserInfoRequest(string AccessToken, string ClientId) : IUnitRequest<UserInfoResponse>;

public class UserInfoRequestHandler(IMediator mediator, IClientCredentialHasher clientCredentialHasher, TimeProvider timeProvider) : IUnitRequestHandler<UserInfoRequest, UserInfoResponse>
{
    public async Task<IUnitResult<UserInfoResponse>> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        var clients = await mediator.Send(new FindClientQuery(request.ClientId), cancellationToken);
        ClientDetailResponse? clientDto;
        if (!clients.IsSuccess || (clientDto = clients.Result?.FirstOrDefault()) is null)
        {
            return UnitResult.NotFound<UserInfoResponse>(request.ClientId).As<UserInfoResponse>();
        }

        var hash = clientCredentialHasher.Hash(request.AccessToken, clientDto);
        var utcNow = timeProvider.GetUtcNow();
        var accessTokens = await mediator.Send(new FindAccessTokenQuery(hash, clientDto.Id, ValidFrom: utcNow, ValidTo: utcNow), cancellationToken);

        AccessTokenDto? accessToken;
        if (!accessTokens.IsSuccess || (accessToken = accessTokens.Result?.OrderByDescending(x => x.ValidFrom).FirstOrDefault()) is null)
        {
            return new UnitResult(new UnauthorizedAccessException()).As<UserInfoResponse>();
        }

        //TODO get associated user and return response
        var users = await mediator.Send(new FindUserQuery(accessToken!.Id), cancellationToken);
        return UnitResult.FromResult(new UserInfoResponse("", "", "", ""), UnitAction.Get);
    }
}