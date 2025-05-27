namespace IDFCR.Shared.Abstractions;

public interface IRoleDescriptorBuilder
{
    IRoleDescriptorBuilder SetCategory(RoleCategory category);
    IRoleDescriptorBuilder AddDisplayName(string displayName);
    IRoleDescriptorBuilder AddDescription(string description);
    IRoleDescriptorBuilder Privileged(bool isPrivilege = true);
    IRoleDescriptor Build();
}
