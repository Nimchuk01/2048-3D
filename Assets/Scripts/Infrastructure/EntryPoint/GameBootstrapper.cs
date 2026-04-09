using System.Collections.Generic;
using Core.Services.Initialization;
using Cysharp.Threading.Tasks;
using GameLogic.GameLoop.Core.Interfaces;
using GameLogic.GameLoop.States;
using Zenject;

namespace Infrastructure.EntryPoint
{
    public class GameBootstrapper : IInitializable
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly List<IInitializableAsync> _initializableServices;

        public GameBootstrapper(IGameStateMachine stateMachine, List<IInitializableAsync> initializableServices)
        {
            _stateMachine = stateMachine;
            _initializableServices = initializableServices;
        }

        public async void Initialize()
        {
            await InitializeServices();
            
            _stateMachine.Enter<CoreState>();
        }
        
        private async UniTask InitializeServices()
        {
            foreach (IInitializableAsync service in _initializableServices) 
                await service.InitializeAsync();
        }
    }
}