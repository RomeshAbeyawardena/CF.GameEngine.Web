namespace IDFCR.Shared.Abstractions;

public enum RoleRequirementType
{
    None = 0,
    All = 1,
    Some = 2
}

public interface IRoleRequirement
{
    IEnumerable<string> Roles { get; }
    RoleRequirementType RoleRequirementType { get; }
}
