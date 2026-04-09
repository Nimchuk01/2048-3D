using Core.Enums;
using Core.Enums.Scene;
using Core.Services.Scenes;
using Core.UI.Management;
using Cysharp.Threading.Tasks;
using GameLogic.GameLoop.States.Interfaces;
using Presentation.UI.Views.Screens.MainMenu;

namespace GameLogic.GameLoop.States
{
    public class MetaState : IState
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly UIManager _uiManager;

        public MetaState(ISceneLoaderService sceneLoaderService, UIManager uiManager)
        {
            _sceneLoaderService = sceneLoaderService;
            _uiManager = uiManager;
        }

        public async void Enter() => 
            await _sceneLoaderService.LoadSceneAsync(SceneType.Meta, postLoadLogic: LoadMetaDependencies);

        public void Exit() { }

        private async UniTask LoadMetaDependencies()
        {
            // Curtain progress is 60% complete
            
            _uiManager.OpenScreen<MainMenuViewModel>();
        }
    }   
}