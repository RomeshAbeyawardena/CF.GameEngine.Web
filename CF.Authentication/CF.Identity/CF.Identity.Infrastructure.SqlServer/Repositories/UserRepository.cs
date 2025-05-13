using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class UserRepository(TimeProvider timeProvider, CFIdentityDbContext context) : RepositoryBase<IUser, DbUser, UserDto>(timeProvider, context), IUserRepository
{
    public async Task<IUnitResult<UserDto>> FindUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await FindAsync(cancellationToken, [id]);
        if (user is null)
        {
            return UnitResult.NotFound<UserDto>(id);
        }

        return UnitResult.FromResult(user);
    }

    public async Task<IUnitResultCollection<UserDto>> FindUsersAsync(IUserFilter filter, CancellationToken cancellationToken)
    {
        var result = await Set<DbUser>(filter)
            .Where(new UserFilter(filter).ApplyFilter(Builder))
            .ToListAsync(cancellationToken);

        return UnitResultCollection.FromResult(result.Select(x => x.Map<UserDto>()).ToList());
    }
}
