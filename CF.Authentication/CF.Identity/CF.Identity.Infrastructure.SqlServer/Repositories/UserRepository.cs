using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using CF.Identity.Infrastructure.SqlServer.Transforms;
using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class UserRepository(IFilter<IUserFilter, DbUser> userFilter, TimeProvider timeProvider, 
    CFIdentityDbContext context, IUserPIIProtection userCredentialProtectionProvider, ICommonNameRepository commonNameRepository) 
    : RepositoryBase<IUser, DbUser, UserDto>(timeProvider, context), IUserRepository
{
    private async Task EnsureUserPIIProtectionIsPrimed(Guid clientId, CancellationToken cancellationToken)
    {
        var dbClient = await Context.Clients.FindAsync([clientId], cancellationToken)
            ?? throw new EntityNotFoundException(typeof(DbClient), clientId);
        userCredentialProtectionProvider.Client = dbClient;
    }

    protected override async Task OnAddAsync(DbUser db, UserDto source, CancellationToken cancellationToken)
    {
        await EnsureUserPIIProtectionIsPrimed(db.ClientId, cancellationToken);
        userCredentialProtectionProvider.Protect(db);
        await UserTransformer.Transform(source, commonNameRepository, cancellationToken, db);
        await base.OnAddAsync(db, source, cancellationToken);
    }

    //it is assumed the data recieved is unencrypted, we take no assurance that the data is unencrypted as we can confirm anything we've given to the consumer is unencrypted.
    protected override async Task OnUpdateAsync(DbUser db, UserDto source, CancellationToken cancellationToken)
    {
        await EnsureUserPIIProtectionIsPrimed(db.ClientId, cancellationToken);

        userCredentialProtectionProvider.Protect(db);
        await UserTransformer.Transform(source, commonNameRepository, cancellationToken, db);
        await base.OnUpdateAsync(db, source, cancellationToken);
    }

    public async Task<IUnitResult<UserDto>> FindUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundUser = await Context.Users
            .AsNoTracking()
            .Include(x => x.FirstCommonName)
            .Include(x => x.MiddleCommonName)
            .Include(x => x.LastCommonName)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (foundUser is null)
        {
            return UnitResult.NotFound<UserDto>(id);
        }

        await EnsureUserPIIProtectionIsPrimed(foundUser.ClientId, cancellationToken);
        userCredentialProtectionProvider.Unprotect(foundUser);
        var user = foundUser.Map<UserDto>();

        var client = await Context.Clients.FindAsync([user.ClientId], cancellationToken);

        if (client is null)
        {
            return UnitResult.NotFound<UserDto>(user.ClientId);
        }

        return UnitResult.FromResult(user);
    }

    public async Task<IUnitResultCollection<UserDto>> FindUsersAsync(IUserFilter filter, CancellationToken cancellationToken)
    {
        var externalFilter = await userFilter.ApplyFilterAsync(Builder, filter, cancellationToken);
        var result = await Set<DbUser>(filter)
            .Include(x => x.Client)
             .Include(x => x.FirstCommonName)
             .Include(x => x.MiddleCommonName)
            .Include(x => x.LastCommonName)
            .Where(externalFilter)
            .ToListAsync(cancellationToken);

        if (result.Count > 0)
        {
            await EnsureUserPIIProtectionIsPrimed(result.First().ClientId, cancellationToken);

            var mappedResult = MapTo(result, (db, x) =>
            {
                userCredentialProtectionProvider.Unprotect(db);
            });

            return UnitResultCollection.FromResult(mappedResult);
        }

        return UnitResultCollection.FromResult(Array.Empty<UserDto>());
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


    public async Task<IUnitResult<Guid>> AnonymiseUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        //we need the database record directly without additional mapping or PII protection
        var dbUser = await Context.Users.FindAsync([id], cancellationToken);

        if (dbUser is null)
        {
            return UnitResult.Failed<Guid>(new EntityNotFoundException(typeof(DbUser), id));
        }

        var anonName = (await commonNameRepository.GetAnonymisedRowRawAsync(false, cancellationToken)).GetResultOrDefault();

        if (anonName is null)
        {
            return UnitResult.Failed<Guid>(new EntityNotFoundException(typeof(DbCommonName), "Anonymised Common Name"));
        }

        dbUser.FirstCommonNameId = anonName.Id;
        dbUser.MiddleCommonNameId = anonName.Id;
        dbUser.LastCommonNameId = anonName.Id;
        dbUser.EmailAddress = string.Empty;
        dbUser.Username = string.Empty;
        dbUser.HashedPassword = string.Empty;
        dbUser.AnonymisedTimestamp = TimeProvider.GetUtcNow();

        return UnitResult.FromResult(dbUser.Id, UnitAction.Update);
    }
}
