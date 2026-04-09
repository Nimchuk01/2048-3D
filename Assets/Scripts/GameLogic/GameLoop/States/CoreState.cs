using Core.Enums.Scene;
using Core.Services.Scenes;
using Core.UI.Management;
using Cysharp.Threading.Tasks;
using GameLogic.GameLoop.States.Interfaces;
using Presentation.UI.Views.Screens.Gameplay;

namespace GameLogic.GameLoop.States
{
    public class CoreState : IState
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly UIManager _uiManager;

        public CoreState(ISceneLoaderService sceneLoaderService, UIManager uiManager)
        {
            _sceneLoaderService = sceneLoaderService;
            _uiManager = uiManager;
        }

        public async void Enter() => 
            await _sceneLoaderService.LoadSceneAsync(SceneType.Core, postLoadLogic: LoadCoreDependencies);

        public void Exit() { }

        private async UniTask LoadCoreDependencies()
        {
            // Curtain progress is 60% complete
            
            _uiManager.OpenScreen<GameplayHUDViewModel>();
        }
    }
}