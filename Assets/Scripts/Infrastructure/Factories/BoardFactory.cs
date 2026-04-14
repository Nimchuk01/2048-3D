using System.Collections.Generic;
using System.Linq;
using Core.Factories;
using Core.Services.AssetManagement;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using Domain.StaticData.Gameplay.Board;
using Domain.StaticData.Gameplay.Cubes;
using Presentation.Gameplay.Board;
using Presentation.Gameplay.Cubes;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Infrastructure.Factories
{
    public class BoardFactory : IBoardFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly ICubeFactory _cubeFactory;
        private readonly DiContainer _container;

        public BoardFactory(
            IStaticDataService staticDataService, 
            IAddressablesLoaderService addressablesLoaderService,
            ICubeFactory cubeFactory,
            DiContainer container)
        {
            _staticDataService = staticDataService;
            _addressablesLoaderService = addressablesLoaderService;
            _cubeFactory = cubeFactory;
            _container = container;
        }

        public async UniTask<BoardEntity> CreateBoard()
        {
            BoardStaticData config = _staticDataService.BoardConfig;
            
            GameObject prefab = await _addressablesLoaderService.Load<GameObject>(config.BoardPrefab);
            
            GameObject board = Object.Instantiate(prefab);
            
            _container.InjectGameObject(board);
            
            BoardEntity boardEntity = board.GetComponent<BoardEntity>();
            
            await SpawnCubes(boardEntity);
            
            return boardEntity;
        }
        
        private async UniTask SpawnCubes(BoardEntity board)
        {
            CubeStaticData cubeConfig = _staticDataService.CubeConfig;
            int cubeCount = Random.Range(cubeConfig.MinCubeCount, cubeConfig.MaxCubeCount);
            
            List<Transform> availablePoints = board.StartCubeSpawnPoints.ToList();
            
            for (int i = 0; i < cubeCount; i++)
            {
                if (availablePoints.Count == 0)
                    break;
                
                int randomIndex = Random.Range(0, availablePoints.Count);
                Transform spawnPoint = availablePoints[randomIndex];
                availablePoints.RemoveAt(randomIndex);
                
                CubeEntity cube = await _cubeFactory.CreateCube(board.CubesParent, isBoardCube: true);
                cube.transform.position = spawnPoint.position;
            }
        }
    }
}