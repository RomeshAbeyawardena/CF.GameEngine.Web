namespace IDFCR.Shared.Abstractions.Roles;

internal class RoleRequirement(Func<IEnumerable<string>> action, RoleRequirementType roleRequirementType) : RoleRequirementBase(action, roleRequirementType);
