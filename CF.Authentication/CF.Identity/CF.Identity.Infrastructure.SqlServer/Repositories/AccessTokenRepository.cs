using CF.Identity.Infrastructure.Features.AccessToken;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class AccessTokenRepository(TimeProvider timeProvider, CFIdentityDbContext context)
    : RepositoryBase<IAccessToken, DbAccessToken, AccessTokenDto>(timeProvider, context), IAccessTokenRepository
{
    public async Task<IUnitResultCollection<AccessTokenDto>> GetAccessTokensAsync(IAccessTokenFilter filter, CancellationToken cancellationToken)
    {
        var result = await Set<DbAccessToken>(filter).Where(new AccessTokenFilter(filter)
            .ApplyFilter(Builder)).ToListAsync(cancellationToken);

        return UnitResultCollection.FromResult(MapTo(result).ToList());
    }
}
