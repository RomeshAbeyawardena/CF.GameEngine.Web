namespace IDFCR.Shared.Hashers;

public interface IUniqueContentHasher
{
    public string CreateHash<T>(T value);
}
