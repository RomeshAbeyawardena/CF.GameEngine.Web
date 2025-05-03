namespace StateManagent.Web.Infrastructure.Hashers;

public interface IUniqueContentHasher
{
    public string CreateHash<T>(T value);
}
