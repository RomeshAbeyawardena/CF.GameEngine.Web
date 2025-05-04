using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Post;

public record PostElementCommand(ElementDto Element) : IRequest<IUnitResult<Guid>>;
