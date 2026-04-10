using Core.Factories;
using Core.Services.AssetManagement;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using Domain.StaticData.Gameplay.Cubes;
using GameLogic.Gameplay.Cubes;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories
{
    public class CubeFactory : ICubeFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly DiContainer _container;

        public CubeFactory(
            IStaticDataService staticDataService,
            IAddressablesLoaderService addressablesLoaderService,
            DiContainer container)
        {
            _staticDataService = staticDataService;
            _addressablesLoaderService = addressablesLoaderService;
            _container = container;
        }

        public async UniTask<CubeEntity> CreateCube(Transform parent)
        {
            CubeStaticData config = _staticDataService.CubeConfig;

            GameObject prefab = await _addressablesLoaderService.Load<GameObject>(config.CubePrefab);

            GameObject cubeObject = Object.Instantiate(prefab, parent);
            cubeObject.transform.localPosition = Vector3.zero;
            cubeObject.transform.localRotation = Quaternion.identity;

            _container.InjectGameObject(cubeObject);

            CubeEntity cube = cubeObject.GetComponent<CubeEntity>();
            int value = GetInitialValue(config);
            cube.Initialize(value);

            return cube;
        }

        private int GetInitialValue(CubeStaticData config)
        {
            return Random.value < config.SpawnChanceFor2 ? 2 : 4;
        }
    }
}
