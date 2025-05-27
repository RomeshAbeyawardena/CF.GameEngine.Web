namespace IDFCR.Shared.Abstractions;

public interface IRoleRegistrar : IEnumerable<IRoleDescriptor>
{
    string? Prefix { get; set; }

    bool TryRegisterRole(string roleName, IRoleDescriptor roleDescriptor, out string key);
    bool TryRegisterRole(string roleName, RoleCategory roleCategory, Action<IRoleDescriptorBuilder> buildRole);
    bool TryRegisterRole(string roleName, Action<IRoleDescriptorBuilder> buildRole);
}
