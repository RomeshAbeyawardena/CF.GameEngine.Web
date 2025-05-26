using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Records;
using IDFCR.Shared.Mediatr;

using IRoleRequirement = IDFCR.Shared.Abstractions.IRoleRequirement;
using RoleRequirementType = IDFCR.Shared.Abstractions.RoleRequirementType;

namespace CF.Identity.Api.Features.Scopes.Get;

public record FindScopesQuery() : 
    MappableBase<IScopeFilter>,
    IUnitRequestCollection<ScopeDto>, IScopeFilter, IRoleRequirement
{
    public static FindScopesQuery Instance(Guid? clientId = null, 
        Guid? userId = null,
        string? key = null, 
        IEnumerable<string>? keys = null,
        bool includePrivilegedScoped = false, 
        bool byPass = false) => new()
        {
            Key = key,
            Keys = keys,
            ClientId = clientId,
            UserId = userId,
            IncludePrivilegedScoped = includePrivilegedScoped,
            Bypass = byPass
        };

    public Guid? ClientId { get; set; }
    public Guid? UserId { get; set; }
    public string? Key { get; set; }
    public bool IncludePrivilegedScoped { get; set; }
    public IEnumerable<string>? Keys { get; set; }
    public bool NoTracking { get; set; }
    public bool Bypass { get; set; }

    protected override IScopeFilter Source => this;

    IEnumerable<string> IRoleRequirement.Roles =>  [SystemRoles.GlobalRead, ScopeRoles.ScopeRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
    
    public override void Map(IScopeFilter source)
    {
        throw new NotImplementedException();
    }
}
