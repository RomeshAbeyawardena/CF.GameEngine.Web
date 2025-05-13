using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;

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

    public Task<IUnitResultCollection<UserDto>> FindUsersAsync(IUserFilter filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
