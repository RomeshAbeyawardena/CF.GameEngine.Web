using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.Transforms;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class UserRepository(IFilter<IUserFilter, DbUser> userFilter, TimeProvider timeProvider, 
    CFIdentityDbContext context, IUserCredentialProtectionProvider userCredentialProtectionProvider) 
    : RepositoryBase<IUser, DbUser, UserDto>(timeProvider, context), IUserRepository
{
    protected override async Task OnAddAsync(DbUser db, UserDto source, CancellationToken cancellationToken)
    {
        var dbClient = await Context.Clients.FindAsync([db.ClientId], cancellationToken) 
            ?? throw new EntityNotFoundException(typeof(DbClient), db.ClientId);
        userCredentialProtectionProvider.Protect(source, dbClient, out var hMac);
        await UserTransformer.Transform(source, Context, hMac, cancellationToken, db);
        await base.OnAddAsync(db, source, cancellationToken);
    }

    //it is assumed the data recieved is unencrypted, we take no assurance that the data is unencrypted as we can confirm anything we've given to the consumer is unencrypted.
    protected override async Task OnUpdateAsync(DbUser db, UserDto source, CancellationToken cancellationToken)
    {
        var dbClient = await Context.Clients.FindAsync([db.ClientId], cancellationToken) 
            ?? throw new EntityNotFoundException(typeof(DbClient), db.ClientId);
        userCredentialProtectionProvider.Protect(source, dbClient, out var hMac);
        await UserTransformer.Transform(source, Context, hMac, cancellationToken, db);

        await base.OnUpdateAsync(db, source, cancellationToken);
    }

    public async Task<IUnitResult<UserDto>> FindUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundUser = await Context.Users
            .Include(x => x.FirstCommonName)
            .Include(x => x.MiddleCommonName)
            .Include(x => x.LastCommonName)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (foundUser is null)
        {
            return UnitResult.NotFound<UserDto>(id);
        }

        var user = foundUser.Map<UserDto>();

        var client = await Context.Clients.FindAsync([user.ClientId], cancellationToken);

        if (client is null)
        {
            return UnitResult.NotFound<UserDto>(user.ClientId);
        }

        userCredentialProtectionProvider.Unprotect(user, client, foundUser);

        return UnitResult.FromResult(user);
    }

    public async Task<IUnitResultCollection<UserDto>> FindUsersAsync(IUserFilter filter, CancellationToken cancellationToken)
    {
        userFilter.Filter = filter;
        var externalFilter = await userFilter.ApplyFilterAsync(Builder, filter, cancellationToken);
        var result = await Set<DbUser>(filter)
            .Include(x => x.Client)
             .Include(x => x.FirstCommonName)
             .Include(x => x.MiddleCommonName)
            .Include(x => x.LastCommonName)
            .Where(externalFilter)
            .ToListAsync(cancellationToken);

        var mappedResult = MapTo(result, (db, x) => userCredentialProtectionProvider.Unprotect(x, db.Client, db));

        return UnitResultCollection.FromResult(mappedResult);
    }

    public async Task<IUnitResultCollection<Guid>> SynchroniseScopesAsync(Guid userId, IEnumerable<Guid> scopeIds, CancellationToken cancellationToken)
    {
        var foundUser = await Context.Users
            .Include(x => x.UserScopes).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if(foundUser is null)
        {
            foundUser = Context.Users.Local.FirstOrDefault(x => x.Id == userId);

            if (foundUser is null)
            {
                return UnitResultCollection.Failed<Guid>(new EntityNotFoundException(typeof(UserDto), userId));
            }
        }

        var existingScopeIds = foundUser.UserScopes.Select(us => us.ScopeId).ToHashSet();
        var scopesToAdd = scopeIds
            .Where(id => !existingScopeIds.Contains(id))
            .Select(id => new DbUserScope { UserId = userId, ScopeId = id })
            .ToList();

        foreach (var scope in scopesToAdd)
        {
            foundUser.UserScopes.Add(scope);
        }

        return UnitResultCollection.FromResult(scopesToAdd.Select(x => x.Id), UnitAction.Update);
    }
}
