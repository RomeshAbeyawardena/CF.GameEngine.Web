using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Filters;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public interface IAccessToken : IEditableAccessToken
{
}

public interface IEditableAccessToken : IMappable<IAccessToken>, IAccessTokenDetail, IIdentifer
{
    string ReferenceToken { get; }
    string AccessToken { get; }
    string? RefreshToken { get; }
    DateTimeOffset? SuspendedTimestampUtc { get; }
}

public interface IAccessTokenSummary : IValidity
{
    Guid UserId { get; }
    Guid ClientId { get; }
    string Type { get; }
}

public interface IAccessTokenDetail : IAccessTokenSummary
{
    string? RevokeReason { get; }
    string? RevokedBy { get; }
}