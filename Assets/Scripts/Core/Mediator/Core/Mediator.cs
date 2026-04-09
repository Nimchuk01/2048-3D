using System;
using System.Collections.Generic;
using System.Threading;
using Core.Logging;
using Core.Mediator.Events;
using Core.Mediator.Requests;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.Mediator.Core
{
    public class Mediator : IMediator
    {
        private readonly Dictionary<Type, object> _handlers = new();
        private readonly List<object> _eventListeners = new();
        
        private readonly DiContainer _container;

        public Mediator(DiContainer container) =>
            _container = container;
        
        public UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            InitializeHandlers(_container);
            InitializeEventListeners(_container);
            
            DebugLogger.LogMessage(message: $"Initialized", sender: this);
            return UniTask.CompletedTask;
        }

        public UniTask Send<TRequest>(TRequest request) where TRequest : IRequest
        {
            if (_handlers.TryGetValue(typeof(TRequest), out object handlerObj) && handlerObj is IRequestHandler<TRequest> handler)
            {
                try
                {
                    return handler.Handle(request);
                }
                catch (Exception ex)
                {
                    DebugLogger.LogError(message: $"Exception in handler {handler.GetType().Name}: {ex.Message}", sender: this);
                    return UniTask.CompletedTask;
                }
            }
            
            DebugLogger.LogWarning(message: $"No handler found for {typeof(TRequest).Name}", sender: this);
            return UniTask.CompletedTask;
        }

        public UniTask<TResponse> Send<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
        {
            if (_handlers.TryGetValue(typeof(TRequest), out object handlerObj) && handlerObj is IRequestHandler<TRequest, TResponse> handler)
            {
                try
                {
                    return handler.Handle(request);
                }
                catch (Exception ex)
                {
                    DebugLogger.LogError(message: $"Exception in handler {handler.GetType().Name}: {ex.Message}", sender: this);
                    return UniTask.FromResult(default(TResponse));
                }
            }
            
            DebugLogger.LogWarning(message: $"No handler with response found for {typeof(TRequest).Name}", sender: this);
            return UniTask.FromResult(default(TResponse));
        }

        public async UniTask Publish<TEvent>(TEvent evt) where TEvent : IEvent
        {
            foreach (object listener in _eventListeners)
            {
                if (listener is IEventListener<TEvent> typedListener)
                {
                    try
                    {
                        await typedListener.OnEvent(evt);
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.LogError(message: $"Exception in listener {typedListener.GetType().Name}: {ex.Message}", sender: this);
                    }
                }
            }
        }

        #region Private methods

        private void InitializeHandlers(DiContainer container)
        {
            List<IRequestHandlerMarker> handlerTypes = container.ResolveAll<IRequestHandlerMarker>();

            foreach (IRequestHandlerMarker handler in handlerTypes)
            {
                Type[] interfaces = handler.GetType().GetInterfaces();
                
                foreach (Type iface in interfaces)
                {
                    if (!iface.IsGenericType) continue;

                    Type def = iface.GetGenericTypeDefinition();
                    if (def == typeof(IRequestHandler<>) || def == typeof(IRequestHandler<,>))
                    {
                        Type requestType = iface.GenericTypeArguments[0];
                        _handlers[requestType] = handler;
                        
                        DebugLogger.LogMessage(message: $"Registered handler: {handler.GetType().Name} for {requestType.Name}", sender: this);
                    }
                }
            }
        }

        private void InitializeEventListeners(DiContainer container)
        {
            List<IEventListenerMarker> listenerObjects = container.ResolveAll<IEventListenerMarker>();

            foreach (IEventListenerMarker listener in listenerObjects)
            {
                Type[] interfaces = listener.GetType().GetInterfaces();

                foreach (Type iface in interfaces)
                {
                    if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IEventListener<>))
                    {
                        _eventListeners.Add(listener);
  
                        DebugLogger.LogMessage(message: $"Registered event listener: {listener.GetType().Name} for {iface.GenericTypeArguments[0].Name}", sender: this);
                    }
                }
            }
        }
        
        #endregion
    }
}
