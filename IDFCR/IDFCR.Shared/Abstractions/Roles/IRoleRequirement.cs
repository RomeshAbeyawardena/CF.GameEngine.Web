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
    Func<IEnumerable<string>> GetRoles { get; } 
    RoleRequirementType RoleRequirementType { get; }
}

public abstract class RoleRequirementBase(Func<IEnumerable<string>> action, RoleRequirementType roleRequirementType) : IRoleRequirement
{
    private readonly Lazy<IEnumerable<string>> _lazyRoles = new(action);

    protected RoleRequirementBase(RoleRequirementType requirementType, params string[] roles) : this(() => roles, requirementType)
    {
        
    }

    public bool Bypass { get; }
    public IEnumerable<string> Roles => _lazyRoles.Value;
    public Func<IEnumerable<string>> GetRoles { get; } = action;
    public RoleRequirementType RoleRequirementType { get; } = roleRequirementType;
}