namespace IDFCR.Shared.Abstractions.Roles;

public interface IRoleRequirement
{
    bool Bypass { get; }
    IEnumerable<string> Roles { get; }
    Func<IEnumerable<string>> GetRoles { get; } 
    RoleRequirementType RoleRequirementType { get; }
}
