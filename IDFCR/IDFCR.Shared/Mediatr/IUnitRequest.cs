using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace IDFCR.Shared.Mediatr;

public interface IUnitRequest : IRequest<IUnitResult>
{
}

public interface IUnitRequestHandler<TRequest> 
    : IRequestHandler<TRequest, IUnitResult>
    where TRequest : IUnitRequest
{

}

public interface IUnitRequest<T> : IRequest<IUnitResult<T>>
{
}

public interface IUnitPagedRequest<T> : IRequest<IUnitPagedResult<T>>
{

}

public interface IUnitPagedRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, IUnitPagedResult<TResponse>>
    where TRequest : IRequest<IUnitPagedResult<TResponse>>
{

}

public interface IUnitRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, IUnitResult<TResponse>>
    where TRequest : IRequest<IUnitResult<TResponse>>
{

}
