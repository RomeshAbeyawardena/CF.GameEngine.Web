using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Introspect;

public record IntrospectQuery(string Token, IClient Client) : IUnitRequest<IntrospectResponse>;
