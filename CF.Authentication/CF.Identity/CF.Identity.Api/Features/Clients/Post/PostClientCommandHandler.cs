﻿using CF.Identity.Api.Features.Clients.Upsert;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.EntityFramework;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.Clients.Post;

public class PostClientCommandHandler(IMediator mediator, ITransactionalUnitOfWork transactionalUnitOfWork) : IUnitRequestHandler<PostClientCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(PostClientCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpsertClientCommand(request.Client, request.Bypass), cancellationToken);
        await transactionalUnitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}