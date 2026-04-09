using System;
using System.Threading;
using Core.Enums;
using Core.Enums.Scene;
using Core.Services.Initialization;
using Core.Services.SceneTransition;
using Core.UI.Root;
using Cysharp.Threading.Tasks;
using GameLogic.GameLoop.Core.Interfaces;
using GameLogic.GameLoop.States;
using R3;

namespace Infrastructure.Services.SceneTransition
{
    public class SceneTransitionWatcher : IInitializableAsync, IDisposable
    {
        private readonly OverlayRootViewModel _rootViewModel;
        private readonly ISceneTransitionService _transitionService;
        private readonly IGameStateMachine _gameStateMachine;
    
        private IDisposable _subscription;

        public SceneTransitionWatcher(OverlayRootViewModel rootViewModel, ISceneTransitionService transitionService, 
            IGameStateMachine gameStateMachine)
        {
            _rootViewModel = rootViewModel;
            _transitionService = transitionService;
            _gameStateMachine = gameStateMachine;
        }
    
        public UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            _subscription = _transitionService.OnTransitionRequested.Subscribe(HandleTransition);
            
            return UniTask.CompletedTask;
        }

        public void Dispose() => _subscription?.Dispose();

        private void HandleTransition(SceneType scene)
        {
            _rootViewModel.Dispose();

            switch (scene)
            {
                case SceneType.Core:
                    _gameStateMachine.Enter<CoreState>();
                    break;
            }
        }
        
    }
}