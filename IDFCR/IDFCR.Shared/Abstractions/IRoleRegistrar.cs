namespace IDFCR.Shared.Abstractions;

public interface IRoleRegistrar : IEnumerable<string>
{
    string? Prefix { get; set; }
    bool TryRegisterRole(string roleName);
    IEnumerable<string> RegisterRoles(params string[] roleNames);
}
