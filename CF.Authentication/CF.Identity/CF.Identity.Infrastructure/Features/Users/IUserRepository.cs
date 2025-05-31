using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;
namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserRepository : IRepository<UserDto>
{
    Task<IUnitResult<UserDto>> FindUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IUnitResultCollection<UserDto>> FindUsersAsync(IUserFilter filter, CancellationToken cancellationToken);
    Task<IUnitResultCollection<Guid>> SynchroniseScopesAsync(Guid userId, IEnumerable<Guid> scopeIds, CancellationToken cancellationToken);
    Task<IUnitResult<Guid>> AnonymiseUserAsync(Guid id, CancellationToken cancellationToken = default);
}
