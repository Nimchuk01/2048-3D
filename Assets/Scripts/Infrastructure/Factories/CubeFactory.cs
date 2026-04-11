using Core.Factories;
using Core.Services.AssetManagement;
using Core.Services.Cube;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using Domain.StaticData.Gameplay.Cubes;
using GameLogic.Gameplay.Cubes;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Infrastructure.Factories
{
    public class CubeFactory : ICubeFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly ICubeService _cubeService;
        private readonly DiContainer _container;

        public CubeFactory(
            IStaticDataService staticDataService,
            IAddressablesLoaderService addressablesLoaderService,
            ICubeService cubeService,
            DiContainer container)
        {
            _staticDataService = staticDataService;
            _addressablesLoaderService = addressablesLoaderService;
            _cubeService = cubeService;
            _container = container;
        }

        public async UniTask<CubeEntity> CreateCube(Transform parent, bool isBoardCube = false)
        {
            CubeStaticData config = _staticDataService.CubeConfig;

            GameObject prefab = await _addressablesLoaderService.Load<GameObject>(config.CubePrefab);

            GameObject cubeObject = Object.Instantiate(prefab, parent);
            cubeObject.transform.localPosition = Vector3.zero;
            cubeObject.transform.localRotation = Quaternion.identity;

            _container.InjectGameObject(cubeObject);

            CubeEntity cube = cubeObject.GetComponent<CubeEntity>();
            int value = isBoardCube ? GetBoardCubeValue(config) : GetSpawnCubeValue(config);
            cube.Initialize(value);
            
            if (!isBoardCube)
                _cubeService.SetActiveCube(cube);

            return cube;
        }

        private int GetBoardCubeValue(CubeStaticData config)
        {
            int[] values = config.PossibleValues;
            if (values == null || values.Length == 0)
                return 2;
            return values[Random.Range(0, values.Length)];
        }

        private int GetSpawnCubeValue(CubeStaticData config)
        {
            return Random.value < config.SpawnChanceFor2 ? 2 : 4;
        }
    }
}
