using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.Transforms;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class UserRepository(TimeProvider timeProvider, CFIdentityDbContext context, IUserCredentialProtectionProvider userCredentialProtectionProvider) 
    : RepositoryBase<IUser, DbUser, UserDto>(timeProvider, context), IUserRepository
{
    protected override async Task OnAddAsync(DbUser db, UserDto source, CancellationToken cancellationToken)
    {
        await UserTransformer.Transform(source, Context, cancellationToken, db);
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
