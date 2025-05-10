using CF.Identity.Infrastructure.Features.Clients;

namespace CF.Identity.Infrastructure;

public interface IClientCredentialHasher
{
    string Hash(string secret, IClient client);
    bool Verify(string secret, IClient client);
}
