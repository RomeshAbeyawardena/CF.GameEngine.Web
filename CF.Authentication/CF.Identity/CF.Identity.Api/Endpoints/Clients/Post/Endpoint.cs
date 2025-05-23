using CF.Identity.Api.Features.Clients;
using CF.Identity.Api.Features.Clients.Post;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Records;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Clients.Post;

public record PostRequest(string Reference, string Name, string? Secret, DateTimeOffset ValidFrom)
    : MappableBase<IClient>
{
    protected override IClient Source => new EditableClientDto {
        Reference = Reference,
        Name = Name,
        SecretHash = Secret,
        ValidFrom = ValidFrom,
        DisplayName = DisplayName,
        ValidTo = ValidTo,
    };
    public string? DisplayName { get; init; }
    public DateTimeOffset? ValidTo { get; init; }

    public override void Map(IClient source)
    {
        throw new NotImplementedException();
    }
}

public static class Endpoint
{
    public static async Task<IResult> SaveClientAsync([FromForm] PostRequest request, 
        IMediator mediator, IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken)
    {
        var data = request.Map<EditableClientDto>();
        
        var result = await mediator.Send(new PostClientCommand(data), cancellationToken);
        return result.NegotiateResult(httpContextAccessor, Endpoints.Url);
    }
}
