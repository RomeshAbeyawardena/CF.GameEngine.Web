namespace IDFCR.Http.Authentication;

public interface IDefaultScopeClaimPolicy
{
    public string DefaultScheme { get; }
    public IEnumerable<string> DefaultScopes { get; }
}

public record DefaultScopeClaimPolicy(string DefaultScheme, params string[] Scopes) : IDefaultScopeClaimPolicy
{
    public IEnumerable<string> DefaultScopes { get; } = Scopes;
}