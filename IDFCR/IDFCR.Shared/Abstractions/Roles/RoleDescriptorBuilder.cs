namespace IDFCR.Shared.Abstractions.Roles;

public class RoleDescriptorBuilder(RoleCategory category = RoleCategory.None) : IRoleDescriptorBuilder
{
    private string? description;
    private string? displayName;
    private bool isPrivileged = false;
    private RoleCategory _category = category;
    private string key = string.Empty;
    string IRoleDescriptorBuilder.Key { get => key; set => key = value; }

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
            Category = _category,
            Description = description,
            DisplayName = displayName
        };
    }

    public IRoleDescriptorBuilder Privileged(bool isPrivileged = true)
    {
        this.isPrivileged = isPrivileged;
        return this;
    }

    public IRoleDescriptorBuilder SetCategory(RoleCategory category)
    {
        _category = category;
        return this;
    }
}