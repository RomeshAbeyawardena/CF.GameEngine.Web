using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public interface IAccessTokenRepository : IRepository<AccessTokenDto>
{
    Task<IUnitResultCollection<AccessTokenDto>> GetAccessTokensAsync(IAccessTokenFilter accessTokenFilter, CancellationToken cancellationToken);
    Task<IUnitResultCollection<Guid>> BulkExpireAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}
