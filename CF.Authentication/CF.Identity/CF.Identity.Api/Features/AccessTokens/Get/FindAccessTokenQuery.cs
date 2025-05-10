using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Mediatr;
namespace CF.Identity.Api.Features.AccessTokens.Get;

public record FindAccessTokenQuery(string? ReferenceToken = null, Guid? ClientId = null, string? Type = null, 
    DateTimeOffset? ValidFrom = null, DateTimeOffset? ValidTo = null, bool NoTracking = true)
    : IUnitRequestCollection<AccessTokenDto>, IAccessTokenFilter
{
    
}
