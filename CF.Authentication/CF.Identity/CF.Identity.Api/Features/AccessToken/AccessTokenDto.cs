using CF.Identity.Infrastructure.Features.AccessToken;

namespace CF.Identity.Api.Features.AccessToken;

public class AccessTokenDto : IEditableAccessToken
{
    public string AccessToken { get; set; } = null!;
    public Guid ClientId { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public string ReferenceToken { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public string Type { get; set; } = null!;
}
