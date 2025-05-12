using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public class AccessTokenDto : MappableBase<IAccessToken>, IAccessToken
{
    protected override IAccessToken Source => this;
    public string ReferenceToken { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public Guid ClientId { get; set; }
    public string Type { get; set; } = null!;
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public override void Map(IAccessToken source)
    {
        ReferenceToken = source.ReferenceToken;
        AccessToken = source.AccessToken;
        RefreshToken = source.RefreshToken;
        ClientId = source.ClientId;
        Type = source.Type;
        ValidFrom = source.ValidFrom;
        ValidTo = source.ValidTo;
        Id = source.Id;
        UserId = source.UserId;
    }
}
