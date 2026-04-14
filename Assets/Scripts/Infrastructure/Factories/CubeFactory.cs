using Core.Factories;
using Core.Services.AssetManagement;
using Core.Services.Boosters;
using Core.Services.GameOver;
using Core.Services.ObjectPool;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using Domain.StaticData.Gameplay.Cubes;
using GameLogic.Gameplay.Cubes;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Infrastructure.Factories
{
    public class CubeFactory : ICubeFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly IGameOverService _gameOverService;
        private readonly ICubeTracker _cubeTracker;
        private readonly IObjectPoolFactory _objectPoolFactory;
        private readonly DiContainer _container;
        private IObjectPoolService<CubeEntity> _pool;

        public CubeFactory(
            IStaticDataService staticDataService,
            IAddressablesLoaderService addressablesLoaderService,
            IGameOverService gameOverService,
            ICubeTracker cubeTracker,
            IObjectPoolFactory objectPoolFactory,
            DiContainer container)
        {
            _staticDataService = staticDataService;
            _addressablesLoaderService = addressablesLoaderService;
            _gameOverService = gameOverService;
            _cubeTracker = cubeTracker;
            _objectPoolFactory = objectPoolFactory;
            _container = container;
        }

        public async UniTask<CubeEntity> CreateCube(Transform parent, bool isBoardCube = false, int? value = null)
        {
            CubeStaticData config = _staticDataService.CubeConfig;

            GameObject prefab = await _addressablesLoaderService.Load<GameObject>(config.CubePrefab);
            CubeEntity prefabCube = prefab.GetComponent<CubeEntity>();
            
            _pool = _objectPoolFactory.GetOrCreatePool(prefabCube, parent);
            CubeEntity cube = _pool.Get();
            
            cube.transform.SetParent(parent);
            cube.transform.localPosition = Vector3.zero;
            cube.transform.localRotation = Quaternion.identity;

            _container.InjectGameObject(cube.gameObject);

            int cubeValue = value ?? (isBoardCube ? GetBoardCubeValue(config) : GetSpawnCubeValue(config));
            cube.Initialize(cubeValue);
            
            Material material = GetMaterialForValue(config, cubeValue);
            cube.SetMaterial(material);

            _gameOverService.RegisterCube();
            _cubeTracker.Register(cube);

            return cube;
        }

        public void ReturnCube(CubeEntity cube)
        {
            _gameOverService.UnregisterCube();
            _cubeTracker.Unregister(cube);
            
            var rb = cube.Rigidbody;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
            
            cube.gameObject.SetActive(false);
            _pool?.Return(cube);
        }

        public async UniTask Preload(Transform parent, int count = 20)
        {
            CubeStaticData config = _staticDataService.CubeConfig;
            GameObject prefab = await _addressablesLoaderService.Load<GameObject>(config.CubePrefab);
            CubeEntity prefabCube = prefab.GetComponent<CubeEntity>();
            
            _pool = _objectPoolFactory.GetOrCreatePool(prefabCube, parent);
            _pool.Preload(count);
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
        
        private Material GetMaterialForValue(CubeStaticData config, int value)
        {
            if (config.CubeMaterials == null)
                return null;
                
            foreach (var materialData in config.CubeMaterials)
            {
                if (materialData.Value == value)
                    return materialData.Material;
            }
            
            return null;
        }
    }
}
