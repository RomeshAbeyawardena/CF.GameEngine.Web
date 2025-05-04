using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Post;

public record PostElementCommand(ElementDto Element) : IUnitRequest<Guid>;
