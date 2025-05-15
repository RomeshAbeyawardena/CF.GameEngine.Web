using CF.Identity.Infrastructure.Features.Clients;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserCredentialProtectionProvider
{
    string Hash(string secret, IUser user);
    bool Verify(string secret, string hash, IUser user);
    void Protect(IUser user, IClient? client = null);
    void Unprotect(IUser user, IClient? client = null);
}
