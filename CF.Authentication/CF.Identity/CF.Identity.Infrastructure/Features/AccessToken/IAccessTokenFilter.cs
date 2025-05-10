using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public interface IAccessTokenFilter : IFilter<IAccessTokenFilter>
{
    string? ReferenceToken { get; }
    Guid? ClientId { get; }
    string? Type { get; }
    DateTimeOffset? ValidFrom { get; }
    DateTimeOffset? ValidTo { get; }
}
