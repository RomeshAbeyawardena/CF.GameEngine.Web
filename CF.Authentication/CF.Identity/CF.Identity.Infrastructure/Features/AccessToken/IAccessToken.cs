using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public interface IAccessToken : IEditableAccessToken
{
}

public interface IEditableAccessToken : IMappable<IAccessToken>, IAccessTokenSummary, IIdentifer
{
    string ReferenceToken { get; }
    string AccessToken { get; }
    string? RefreshToken { get; }
}

public interface IAccessTokenSummary : IValidity
{
    Guid UserId { get; }
    Guid ClientId { get; }
    string Type { get; }
}