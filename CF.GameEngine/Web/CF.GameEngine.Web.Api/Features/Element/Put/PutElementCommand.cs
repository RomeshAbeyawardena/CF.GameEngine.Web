using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Put;

public record PutElementCommand(ElementDto Element) : IRequest<IUnitResult<Guid>>;
