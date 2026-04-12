using Core.Services.GameOver;
using Core.Services.Score;
using Core.Services.StaticData;
using GameLogic.GameLoop.Core.Interfaces;
using R3;

namespace Infrastructure.Services.GameOver
{
    public class GameOverService : IGameOverService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ReactiveProperty<bool> _isGameOver = new(false);
        private int _cubeCount;

        public ReadOnlyReactiveProperty<bool> IsGameOver => _isGameOver;

        public GameOverService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void RegisterCube()
        {
            _cubeCount++;
            CheckGameOver();
        }

        public void UnregisterCube()
        {
            _cubeCount--;
        }
        
        private void CheckGameOver()
        {
            int limit = _staticDataService.BoardConfig.MaxCubesLimit;
            if (_cubeCount >= limit)
                _isGameOver.Value = true;
        }
    }
}