using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public interface IAccessTokenFilter : IFilter<IAccessTokenFilter>
{
    string? ReferenceToken { get; }
    Guid? ClientId { get; }
    string? Type { get; }
    DateTimeOffset? ValidFrom { get; }
    DateTimeOffset? ValidTo { get; }
}

public interface IAccessTokenRepository : IRepository<AccessTokenDto>
{
    Task<IUnitResultCollection<AccessTokenDto>> GetAccessTokensAsync(IAccessTokenFilter accessTokenFilter, CancellationToken cancellationToken);
}
