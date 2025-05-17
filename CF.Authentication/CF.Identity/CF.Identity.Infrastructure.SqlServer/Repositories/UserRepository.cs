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
        userFilter.Filter = filter;
        var externalFilter = await userFilter.ApplyFilterAsync(Builder, cancellationToken);
        var result = await Set<DbUser>(filter)
            .Include(x => x.Client)
            .Where(externalFilter)
            .ToListAsync(cancellationToken);

        var mappedResult = MapTo(result, (db, x) => userCredentialProtectionProvider.Unprotect(x, db.Client));

        return UnitResultCollection.FromResult(mappedResult);
    }
}
