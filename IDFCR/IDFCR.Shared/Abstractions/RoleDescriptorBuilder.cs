namespace IDFCR.Shared.Abstractions;

public class RoleDescriptorBuilder(string key) : IRoleDescriptorBuilder
{
    private string? description;
    private string? displayName;
    private bool isPrivileged = false;

    public IRoleDescriptorBuilder AddDescription(string description)
    {
        this.description = description;
        return this;
    }

    public IRoleDescriptorBuilder AddDisplayName(string displayName)
    {
        this.displayName = displayName;
        return this;
    }

    public IRoleDescriptor Build()
    {
        return new DefaultRoleDescriptor(key, isPrivileged)
        {
            Description = description,
            DisplayName = displayName
        };
    }

    public IRoleDescriptorBuilder Privileged(bool isPrivileged = true)
    {
        this.isPrivileged = isPrivileged;
        return this;
    }
}