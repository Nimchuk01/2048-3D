using Core.Factories;
using Core.Services.Boosters;
using Core.Services.GameOver;
using Core.Services.Merge;
using Core.Services.Score;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.Cubes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Services.Merge
{
    public class MergeService : IMergeService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICubeFactory _cubeFactory;
        private readonly IScoreService _scoreService;
        private readonly IGameOverService _gameOverService;
        private readonly ICubeTracker _cubeTracker;

        public MergeService(
            IStaticDataService staticDataService,
            ICubeFactory cubeFactory,
            IScoreService scoreService,
            IGameOverService gameOverService,
            ICubeTracker cubeTracker)
        {
            _staticDataService = staticDataService;
            _cubeFactory = cubeFactory;
            _scoreService = scoreService;
            _gameOverService = gameOverService;
            _cubeTracker = cubeTracker;
        }

        public bool CanMerge(CubeEntity source, CubeEntity target, float relativeVelocityMagnitude)
        {
            if (source.Value != target.Value) 
                return false;
            
            return relativeVelocityMagnitude >= _staticDataService.CubeConfig.MergeVelocityThreshold;
        }

        public void Merge(CubeEntity source, CubeEntity target)
        {
            int newValue = source.Value * 2;
            Vector3 spawnPosition = (source.transform.position + target.transform.position) / 2f;

            DisableCube(source);
            DisableCube(target);

            _scoreService.AddScore(newValue);
            SpawnMergedCube(spawnPosition, newValue, source.transform.parent).Forget();
        }

        private void DisableCube(CubeEntity cube)
        {
            _gameOverService.UnregisterCube();
            _cubeTracker.Unregister(cube);
            cube.gameObject.SetActive(false);
            Object.Destroy(cube.gameObject);
        }

        private async UniTask SpawnMergedCube(Vector3 position, int value, Transform parent)
        {
            CubeEntity newCube = await _cubeFactory.CreateCube(parent, isBoardCube: true);
            newCube.transform.position = position;
            newCube.Initialize(value);
        }
    }
}
