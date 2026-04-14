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
            Vector3 sourceVelocity = source.Rigidbody.linearVelocity;
            Vector3 targetVelocity = target.Rigidbody.linearVelocity;

            DisableCube(source);
            DisableCube(target);

            _scoreService.AddScore(newValue);
            SpawnMergedCube(spawnPosition, newValue, source.transform.parent, sourceVelocity, targetVelocity).Forget();
        }

        private void DisableCube(CubeEntity cube)
        {
            _cubeFactory.ReturnCube(cube);
        }

        private async UniTask SpawnMergedCube(Vector3 position, int value, Transform parent, Vector3 sourceVelocity, Vector3 targetVelocity)
        {
            CubeEntity newCube = await _cubeFactory.CreateCube(parent, isBoardCube: true, value);
            newCube.transform.position = position;
            
            Rigidbody rb = newCube.Rigidbody;
            
            Vector3 impactVelocity = sourceVelocity.magnitude > targetVelocity.magnitude ? sourceVelocity : targetVelocity;
            Vector3 forward = impactVelocity.normalized;
            
            float launchForce = Random.Range(5f, 10f);
            Vector3 impulse = forward * launchForce;
            impulse.y = Random.Range(2f, 5f);
            
            rb.AddForce(impulse, ForceMode.Impulse);
        }
    }
}
