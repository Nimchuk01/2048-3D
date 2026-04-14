using Core.Factories;
using Core.Services.Merge;
using Core.Services.Score;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.Cubes;
using UnityEngine;

namespace Infrastructure.Services.Merge
{
    public class MergeService : IMergeService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICubeFactory _cubeFactory;
        private readonly IScoreService _scoreService;

        public MergeService(
            IStaticDataService staticDataService,
            ICubeFactory cubeFactory,
            IScoreService scoreService)
        {
            _staticDataService = staticDataService;
            _cubeFactory = cubeFactory;
            _scoreService = scoreService;
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
            _cubeFactory.ReturnCube(cube);
        }

        private async UniTask SpawnMergedCube(Vector3 position, int value, Transform parent)
        {
            CubeEntity newCube = await _cubeFactory.CreateCube(parent, isBoardCube: true, value);
            newCube.transform.position = position;
        }
    }
}
