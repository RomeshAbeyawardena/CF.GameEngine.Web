namespace IDFCR.Shared.Abstractions;

public interface IRoleRegistrar : IEnumerable<IRoleDescriptor>
{
    string? Prefix { get; set; }

    bool TryRegisterRole(string roleName, bool isPrivileged = false);
    IEnumerable<string> RegisterRoles(params string[] roleNames);
}
