namespace IDFCR.Shared.Abstractions.Roles;

public interface IRoleDescriptorBuilder
{
    string Key { get; internal set; }
    IRoleDescriptorBuilder SetCategory(RoleCategory category);
    IRoleDescriptorBuilder AddDisplayName(string displayName);
    IRoleDescriptorBuilder AddDescription(string description);
    IRoleDescriptorBuilder Privileged(bool isPrivilege = true);
    IRoleDescriptor Build();
}
