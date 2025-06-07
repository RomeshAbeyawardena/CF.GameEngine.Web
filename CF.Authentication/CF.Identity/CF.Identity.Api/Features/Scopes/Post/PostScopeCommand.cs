using CF.Identity.Api.Features.Scopes.Upsert;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.EntityFramework;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.Scopes.Post;

public record PostScopeCommand(EditableScopeDto Scope, bool Bypass = false) : IUnitRequest<Guid>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<ScopeRoles>(RoleCategory.Write);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}

public class PostScopeCommandHandler(IMediator mediator, ITransactionalUnitOfWork transactionalUnitOfWork) : IUnitRequestHandler<PostScopeCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(PostScopeCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpsertScopeCommand(request.Scope, request.Bypass), cancellationToken);

        await transactionalUnitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}