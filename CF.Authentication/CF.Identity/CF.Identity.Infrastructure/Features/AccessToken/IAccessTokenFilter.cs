using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public interface IAccessTokenFilter : IFilter<IAccessTokenFilter>, IValidityFilter
{
    string? ReferenceToken { get; }
    Guid? ClientId { get; }
    string? Type { get; }
}
