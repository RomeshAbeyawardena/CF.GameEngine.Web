namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserCredentialProtectionProvider
{
    string Hash(string secret, IUser user);
    bool Verify(string secret, string hash, IUser user);
    void Protect(IUser user);
    void Unprotect(IUser user);
}
