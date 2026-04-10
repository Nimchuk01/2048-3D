using Core.Factories;
using Core.Services.AssetManagement;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using Domain.StaticData.Gameplay.Board;
using GameLogic.Gameplay.Board;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories
{
    public class BoardFactory : IBoardFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly DiContainer _container;

        public BoardFactory(
            IStaticDataService staticDataService, 
            IAddressablesLoaderService addressablesLoaderService,
            DiContainer container)
        {
            _staticDataService = staticDataService;
            _addressablesLoaderService = addressablesLoaderService;
            _container = container;
        }

        public async UniTask<BoardEntity> CreateBoard()
        {
            BoardStaticData config = _staticDataService.BoardConfig;
            
            GameObject prefab = await _addressablesLoaderService.Load<GameObject>(config.BoardPrefab);
            
            GameObject board = Object.Instantiate(prefab);
            
            _container.InjectGameObject(board);
            
            return board.GetComponent<BoardEntity>();
        }
    }
}