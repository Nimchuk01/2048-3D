using System;
using Core.Factories;
using Core.Services.Merge;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using Domain.StaticData.Gameplay.Cubes;
using GameLogic.Gameplay.Cubes;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Services.Merge
{
    public class MergeService : IMergeService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ICubeFactory _cubeFactory;

        public MergeService(
            IStaticDataService staticDataService,
            ICubeFactory cubeFactory)
        {
            _staticDataService = staticDataService;
            _cubeFactory = cubeFactory;
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

            SpawnMergedCube(spawnPosition, newValue, source.transform.parent).Forget();
        }

        private void DisableCube(CubeEntity cube)
        {
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
