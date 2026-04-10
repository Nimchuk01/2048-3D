using Core.Enums.Scene;
using Core.Factories;
using Core.Services.Scenes;
using Core.UI.Management;
using Cysharp.Threading.Tasks;
using GameLogic.GameLoop.States.Interfaces;
using GameLogic.Gameplay.Board;
using GameLogic.Gameplay.Cubes;
using Presentation.UI.Views.Screens.Gameplay;

namespace GameLogic.GameLoop.States
{
    public class CoreState : IState
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly UIManager _uiManager;
        private readonly IBoardFactory _boardFactory;
        private readonly ICubeFactory _cubeFactory;

        public CoreState(
            ISceneLoaderService sceneLoaderService,
            UIManager uiManager,
            IBoardFactory boardFactory,
            ICubeFactory cubeFactory)
        {
            _sceneLoaderService = sceneLoaderService;
            _uiManager = uiManager;
            _boardFactory = boardFactory;
            _cubeFactory = cubeFactory;
        }

        public async void Enter() => 
            await _sceneLoaderService.LoadSceneAsync(SceneType.Core, postLoadLogic: LoadCoreDependencies);

        public void Exit() { }

        private async UniTask LoadCoreDependencies()
        {
            BoardEntity board = await _boardFactory.CreateBoard();

            CubeEntity cube = await _cubeFactory.CreateCube(board.CubesParent);

            _uiManager.OpenScreen<GameplayHUDViewModel>();
        }
    }
}