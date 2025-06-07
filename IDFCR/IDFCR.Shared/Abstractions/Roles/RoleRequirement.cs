namespace IDFCR.Shared.Abstractions.Roles;

internal class RoleRequirement(Func<IEnumerable<string>> action, RoleRequirementType roleRequirementType, bool bypass) 
    : RoleRequirementBase(action, roleRequirementType)
{
    public override bool Bypass => bypass;
}
