using System.Diagnostics;

namespace IDFCR.Shared.Abstractions.Roles.Records;

[DebuggerDisplay("Roles = {string.Join(\", \", Roles)}, Bypass = {Bypass}")]
public abstract record RoleRequirementBase(Func<IEnumerable<string>> GetRoles, RoleRequirementType RoleRequirementType = RoleRequirementType.Some) 
    : IRoleRequirement
{
    private readonly Lazy<IEnumerable<string>> _lazyRoles = new(GetRoles);

    protected RoleRequirementBase(RoleRequirementType requirementType, params string[] roles) : this(() => roles, requirementType)
    {

    }

    public abstract bool Bypass { get; init; }
    public IEnumerable<string> Roles => _lazyRoles.Value;
    public Func<IEnumerable<string>> GetRoles { get; } = GetRoles;
    public RoleRequirementType RoleRequirementType { get; } = RoleRequirementType;
}