 using CF.Identity.Infrastructure.Features.AccessToken;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.SPA;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class AccessTokenRepository(IAccessTokenSpaProtection accessTokenProtection, TimeProvider timeProvider, CFIdentityDbContext context)
    : RepositoryBase<IAccessToken, DbAccessToken, AccessTokenDto>(timeProvider, context), IAccessTokenRepository
{
    private async Task PrimeAccessTokenProtection(Guid clientId, CancellationToken cancellationToken)
    {
        var client = await Context.Clients.FindAsync([clientId], cancellationToken)
            ?? throw new EntityNotFoundException(typeof(DbClient), clientId);
        accessTokenProtection.Client = client;
    }

    protected override async Task OnAddAsync(DbAccessToken db, AccessTokenDto source, CancellationToken cancellationToken)
    {
        await PrimeAccessTokenProtection(db.ClientId, cancellationToken);
        accessTokenProtection.Protect(db);
        await base.OnAddAsync(db, source, cancellationToken);
    }

    public async Task<IUnitResultCollection<Guid>> BulkExpireAsync(IEnumerable<Guid> ids, string? revokeReason, string? revokedBy, CancellationToken cancellationToken)
    {
        List<Guid> updatedIds = [];
        foreach(var id in ids)
        {
            //this is low-level to make it cheaper to run;
            var accessToken = await Context.AccessTokens.FindAsync([id], cancellationToken);

            if (accessToken is null)
            {
                continue;
            }

            var utcNow = TimeProvider.GetUtcNow();
            accessToken.SuspendedTimestampUtc = utcNow;
            accessToken.ValidTo = utcNow;
            accessToken.RevokeReason = revokeReason;
            accessToken.RevokedBy = revokedBy;
            updatedIds.Add(id);
        }

        return UnitResultCollection.FromResult(updatedIds, UnitAction.Delete);
    }

    public async Task<IUnitResultCollection<AccessTokenDto>> GetAccessTokensAsync(IAccessTokenFilter filter, CancellationToken cancellationToken)
    {
        var result = await Set<DbAccessToken>(filter).Where(new AccessTokenFilter(filter)
            .ApplyFilter(Builder)).ToListAsync(cancellationToken);

        return UnitResultCollection.FromResult(MapTo(result));
    }
}
