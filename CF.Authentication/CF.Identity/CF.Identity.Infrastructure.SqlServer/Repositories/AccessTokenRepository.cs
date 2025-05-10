using CF.Identity.Infrastructure.Features.AccessToken;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class AccessTokenRepository(TimeProvider timeProvider, CFIdentityDbContext context)
    : RepositoryBase<IAccessToken, DbAccessToken, AccessTokenDto>(timeProvider, context), IAccessTokenRepository
{
    public Task<IUnitResultCollection<AccessTokenDto>> GetAccessTokensAsync(IAccessTokenFilter accessTokenFilter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
