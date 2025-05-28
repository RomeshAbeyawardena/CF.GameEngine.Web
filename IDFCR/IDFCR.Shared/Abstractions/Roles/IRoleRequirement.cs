namespace IDFCR.Shared.Abstractions.Roles;

public enum RoleRequirementType
{
    None = 0,
    All = 1,
    Some = 2
}

public interface IRoleRequirement
{
    bool Bypass { get; }
    IEnumerable<string> Roles { get; }
    RoleRequirementType RoleRequirementType { get; }
}
