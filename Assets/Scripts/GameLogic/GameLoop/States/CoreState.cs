using Core.Enums.Scene;
using Core.Factories;
using Core.Services.Scenes;
using Core.UI.Management;
using Cysharp.Threading.Tasks;
using GameLogic.GameLoop.States.Interfaces;
using Presentation.UI.Views.Screens.Gameplay;
using UnityEngine;

namespace GameLogic.GameLoop.States
{
    public class CoreState : IState
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly UIManager _uiManager;
        private readonly IBoardFactory _boardFactory;

        public CoreState(
            ISceneLoaderService sceneLoaderService,
            UIManager uiManager,
            IBoardFactory boardFactory)
        {
            _sceneLoaderService = sceneLoaderService;
            _uiManager = uiManager;
            _boardFactory = boardFactory;
        }

        public async void Enter() => 
            await _sceneLoaderService.LoadSceneAsync(SceneType.Core, postLoadLogic: LoadCoreDependencies);

        public void Exit() { }

        private async UniTask LoadCoreDependencies()
        {
            GameObject board = await _boardFactory.CreateBoard();
            
            _uiManager.OpenScreen<GameplayHUDViewModel>();
        }
    }
}