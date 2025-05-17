using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class UserRepository(TimeProvider timeProvider, CFIdentityDbContext context, IUserCredentialProtectionProvider userCredentialProtectionProvider) 
    : RepositoryBase<IUser, DbUser, UserDto>(timeProvider, context), IUserRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// <para>false and null: Did not resolve</para>
    /// <para>false and not null: Resolved with an existing entity</para>
    /// <para>true and not null: Resolved with a new entity</para>
    /// </returns>
    private async Task<(bool, DbCommonName?)> ResolveNameAsync(string name, CancellationToken cancellationToken)
    {
        var normalised = name.Trim();
        var upper = normalised.ToUpperInvariant();

        var foundName = await Context.CommonNames
            .FirstOrDefaultAsync(x => x.Value == upper, cancellationToken);

        if (foundName is null)
        {
            var newName = new DbCommonName
            {
                Value = upper,
                ValueNormalised = normalised
            };
            Context.CommonNames.Add(newName);
            await Context.SaveChangesAsync(cancellationToken);
            return (true, newName);
        }

        return (false, foundName);
    }

    private async Task SetCommonNameAsync(string? lookupName, Action<DbCommonName> setEntityRef, Action<Guid> setIdRef, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(lookupName))
        {
            var (isNew, name) = await ResolveNameAsync(lookupName, cancellationToken);
            if (name is null)
                throw new EntityNotFoundException(typeof(DbCommonName), lookupName);

            if (isNew)
                setEntityRef(name);
            else
                setIdRef(name.Id);
        }
    }


    protected override async Task OnAddAsync(DbUser db, UserDto source, CancellationToken cancellationToken)
    {
        await SetCommonNameAsync(db.LookupFirstName, name => db.FirstCommonName = name, id => db.FirstCommonNameId = id, cancellationToken);
        await SetCommonNameAsync(db.LookupMiddleName, name => db.MiddleCommonName = name, id => db.MiddleCommonNameId = id, cancellationToken);
        await SetCommonNameAsync(db.LookupLastName, name => db.LastCommonName = name, id => db.LastCommonNameId = id, cancellationToken);

        await base.OnAddAsync(db, source, cancellationToken);
    }

    public async Task<IUnitResult<UserDto>> FindUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await FindAsync(cancellationToken, [id]);
        if (user is null)
        {
            return UnitResult.NotFound<UserDto>(id);
        }

        var client = await Context.Clients.FindAsync([user.ClientId], cancellationToken);

        if (client is null)
        {
            return UnitResult.NotFound<UserDto>(user.ClientId);
        }

        userCredentialProtectionProvider.Unprotect(user, client);

        return UnitResult.FromResult(user);
    }

    public async Task<IUnitResultCollection<UserDto>> FindUsersAsync(IUserFilter filter, CancellationToken cancellationToken)
    {
        var result = await Set<DbUser>(filter)
            .Include(x => x.Client)
            .Where(new UserFilter(filter).ApplyFilter(Builder))
            .ToListAsync(cancellationToken);

        var mappedResult = MapTo(result, (db, x) => userCredentialProtectionProvider.Unprotect(x, db.Client));

        return UnitResultCollection.FromResult(mappedResult);
    }
}
