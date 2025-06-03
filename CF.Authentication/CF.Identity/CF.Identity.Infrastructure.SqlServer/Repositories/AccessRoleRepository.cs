using CF.Identity.Infrastructure.Features.AccessRoles;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class AccessRoleRepository(TimeProvider timeProvider, CFIdentityDbContext context)
    : RepositoryBase<IAccessRole, DbAccessRole, AccessRoleDto>(timeProvider, context), IAccessRoleRepository
{
    public async Task<IUnitResult<AccessRoleDto>> FindAccessTokenAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await FindAsync(cancellationToken, id);
        if (result is null)
        {
            return UnitResult.NotFound<AccessRoleDto>(id);
        }

        return UnitResult.FromResult(result);
    }

    public async Task<IUnitResultCollection<AccessRoleDto>> GetAccessRolesAsync(IAccessRoleFilter filter, CancellationToken cancellationToken)
    {
        var queryFilter = new AccessRoleFilter(filter);
        var query = await Set<DbAccessRole>(filter)
            .Where(queryFilter.ApplyFilter(Builder))
            .ToListAsync(cancellationToken);

        return UnitResultCollection.FromResult(query.Select(x => x.Map<AccessRoleDto>()));
    }
}
