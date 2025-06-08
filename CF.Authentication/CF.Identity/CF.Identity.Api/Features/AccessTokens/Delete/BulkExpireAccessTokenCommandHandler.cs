using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessTokens.Delete;

public class BulkExpireAccessTokenCommandHandler(IAccessTokenRepository accessTokenRepository) : IUnitRequestHandler<BulkExpireAccessTokenCommand>
{
    public async Task<IUnitResult> Handle(BulkExpireAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await accessTokenRepository.BulkExpireAsync(request.AccessTokenIds,
            request.RevokeReason, request.RevokedBy, cancellationToken);

        await accessTokenRepository.SaveChangesAsync(cancellationToken);

        return result;
    }
}
