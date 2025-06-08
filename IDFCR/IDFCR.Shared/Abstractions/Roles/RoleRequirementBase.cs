using System.Diagnostics;

namespace IDFCR.Shared.Abstractions.Roles;

[DebuggerDisplay("Roles = {string.Join(\", \", Roles)}, Bypass = {Bypass}")]
[RoleRequirement("thing", RoleRequirementType.Some)]
public abstract class RoleRequirementBase(
    Func<IEnumerable<string>> action, RoleRequirementType roleRequirementType = RoleRequirementType.Some) : IRoleRequirement
{
    private readonly Lazy<IEnumerable<string>> _lazyRoles = new(action);

    protected RoleRequirementBase(RoleRequirementType requirementType, params string[] roles) : this(() => roles, requirementType)
    {
        
    }

    public abstract bool Bypass { get; }
    public IEnumerable<string> Roles => _lazyRoles.Value;
    public Func<IEnumerable<string>> GetRoles { get; } = action;
    public RoleRequirementType RoleRequirementType { get; } = roleRequirementType;
}
