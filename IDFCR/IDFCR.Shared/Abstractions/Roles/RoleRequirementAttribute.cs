namespace IDFCR.Shared.Abstractions.Roles;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RoleRequirementAttribute(string actionName, RoleRequirementType roleRequirementType = RoleRequirementType.Some) : Attribute
{
    public string ActionName { get; } = actionName;
    public RoleRequirementType RoleRequirementType { get; } = roleRequirementType;
}
