using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using IDFCR.Shared.Extensions;
namespace CF.Identity.Api.Features.AccessToken.Get;

public class FindAccessTokenQueryHandler(IAccessTokenRepository accessTokenRepository) : IUnitRequestCollectionHandler<FindAccessTokenQuery, AccessTokenDto>
{
    public async Task<IUnitResultCollection<AccessTokenDto>> Handle(FindAccessTokenQuery request, CancellationToken cancellationToken)
    {
        var result = await accessTokenRepository.GetAccessTokensAsync(request, cancellationToken);
        return result.Convert(x => x.Map<AccessTokenDto>());
    }
}
