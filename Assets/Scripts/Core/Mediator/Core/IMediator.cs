using Core.Mediator.Events;
using Core.Mediator.Requests;
using Core.Services.Initialization;
using Cysharp.Threading.Tasks;

namespace Core.Mediator.Core
{
    public interface IMediator : IInitializableAsync
    {
        UniTask Send<TRequest>(TRequest request) where TRequest : IRequest;
        UniTask<TResponse> Send<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>;
        UniTask Publish<TEvent>(TEvent evt) where TEvent : IEvent;
    }
}