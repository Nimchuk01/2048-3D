using Core.Factories;
using Core.Services.AssetManagement;
using Core.Services.Board;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using Domain.StaticData.Gameplay.Board;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories
{
    public class BoardFactory : IBoardFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly IBoardService _boardService;
        private readonly DiContainer _container;

        public BoardFactory(
            IStaticDataService staticDataService, 
            IAddressablesLoaderService addressablesLoaderService,
            IBoardService boardService,
            DiContainer container)
        {
            _staticDataService = staticDataService;
            _addressablesLoaderService = addressablesLoaderService;
            _boardService = boardService;
            _container = container;
        }

        public async UniTask<GameObject> CreateBoard(Transform parent = null)
        {
            BoardStaticData config = _staticDataService.BoardConfig;
            
            _boardService.Initialize(config.CubeSpawnPosition);
            
            GameObject prefab = await _addressablesLoaderService.Load<GameObject>(config.BoardPrefab);
            
            GameObject board = Object.Instantiate(prefab, parent);
            
            _container.InjectGameObject(board);
            
            return board;
        }
    }
}