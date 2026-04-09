using Cysharp.Threading.Tasks;

namespace Core.Mediator.Requests
{
    public interface IRequestHandler<in TRequest> : IRequestHandlerMarker where TRequest : IRequest
    {
        UniTask Handle(TRequest request);
    }
    
    public interface IRequestHandler<in TRequest, TResponse> : IRequestHandlerMarker where TRequest : IRequest<TResponse>
    {
        UniTask<TResponse> Handle(TRequest request);
    }
}