using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessTokens;

public record UpsertAccessTokenCommand(AccessTokenDto AccessToken) : IUnitRequest<Guid>;

public class UpsertAccessTokenCommandHandler(IAccessTokenRepository accessTokenRepository) : IUnitRequestHandler<UpsertAccessTokenCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(UpsertAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await accessTokenRepository.UpsertAsync(request.AccessToken.Map<Infrastructure.Features.AccessToken.AccessTokenDto>(), cancellationToken);
        await accessTokenRepository.SaveChangesAsync(cancellationToken);
        return result;
    }
}