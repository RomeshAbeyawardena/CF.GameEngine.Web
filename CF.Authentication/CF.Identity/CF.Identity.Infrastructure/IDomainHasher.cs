namespace CF.Identity.Infrastructure;

public interface IDomainHasher<T>
{
    string Hash(string secret, T value);
    bool Verify(string secret, T value);
}
