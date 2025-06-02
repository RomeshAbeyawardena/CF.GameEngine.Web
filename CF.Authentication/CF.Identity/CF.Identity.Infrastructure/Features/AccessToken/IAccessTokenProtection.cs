using CF.Identity.Infrastructure.Features.Clients;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public interface IAccessTokenProtection
{
    string GetHashedAccessToken(string accessToken);
    bool VerifyAccessToken(string accessToken);
    IClient Client { get; set; }
}
