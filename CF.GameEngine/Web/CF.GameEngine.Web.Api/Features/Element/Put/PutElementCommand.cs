using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Put;

public record PutElementCommand(ElementDto Element) : IUnitRequest<Guid>;
