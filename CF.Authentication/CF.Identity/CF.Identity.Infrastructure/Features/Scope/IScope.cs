using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Scope;

public interface IScope : IEditableScope
{

}

public interface IEditableScope : IMappable<IScope>, IScopeDetail, IIdentifer
{
    string Key { get; }

}

public interface IScopeSummary
{
    Guid? ClientId { get; }
    string Name { get; }
    string? Description { get; }
}

public interface IScopeDetail : IScopeSummary
{
    //only system enabled clients can execute any of these scopes
    bool IsPrivileged { get; }
}