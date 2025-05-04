using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Paging;

namespace CF.Identity.Infrastructure.Features.Clients;

public interface IPagedClientFilter : IClientFilter, IPagedQuery;
public interface IClientFilter : IFilter<IClient>
{
}
