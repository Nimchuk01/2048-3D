using Core.Enums.Scene;
using Core.Factories;
using Core.Services.Scenes;
using Core.Services.Cube;
using Core.UI.Management;
using Cysharp.Threading.Tasks;
using GameLogic.GameLoop.States.Interfaces;
using GameLogic.Gameplay.Board;
using Presentation.UI.Views.Screens.Gameplay;

namespace GameLogic.GameLoop.States
{
    public class CoreState : IState
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly UIManager _uiManager;
        private readonly IBoardFactory _boardFactory;
        private readonly ICubeService _cubeService;

        public CoreState(
            ISceneLoaderService sceneLoaderService,
            UIManager uiManager,
            IBoardFactory boardFactory,
            ICubeService cubeService)
        {
            _sceneLoaderService = sceneLoaderService;
            _uiManager = uiManager;
            _boardFactory = boardFactory;
            _cubeService = cubeService;
        }

        public async void Enter() => 
            await _sceneLoaderService.LoadSceneAsync(SceneType.Core, postLoadLogic: LoadCoreDependencies);

        public void Exit() { }

        private async UniTask LoadCoreDependencies()
        {
            BoardEntity board = await _boardFactory.CreateBoard();
            
            _cubeService.SetCubesParent(board.CubesParent);
            await _cubeService.SpawnPlayerCube();
            
            _uiManager.OpenScreen<GameplayHUDViewModel>();
        }
    }
}