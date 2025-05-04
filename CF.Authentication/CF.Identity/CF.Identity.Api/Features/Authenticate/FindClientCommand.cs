using MediatR;

namespace CF.Identity.Api.Features.Authenticate;

public record FindClientCommand(IClientFilter ClientFilter) : IRequest<Client?>
{

}
