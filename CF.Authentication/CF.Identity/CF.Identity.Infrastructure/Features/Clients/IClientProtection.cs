namespace CF.Identity.Infrastructure.Features.Clients;

public interface IClientProtection
{
    public string HashSecret(string secret);
    bool VerifySecret(IClient client, string secret);
}
