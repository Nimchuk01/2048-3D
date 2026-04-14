using System;
using System.Collections.Generic;
using Core.Enums.UI;
using Core.Enums.UI.Extensions;
using Core.Factories.UI;
using Core.Services.AssetManagement;
using Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using Domain.StaticData.UI.Canvas;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories.UI
{
    public class CanvasFactory : ICanvasFactory, IDisposable
    {
        private readonly Dictionary<CanvasType, Canvas> _canvases = new();
        
        private readonly IStaticDataService _staticDataService;
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly DiContainer _container;

        public CanvasFactory(IStaticDataService staticDataService, IAddressablesLoaderService addressablesLoaderService, 
            DiContainer container)
        {
            _staticDataService = staticDataService;
            _addressablesLoaderService = addressablesLoaderService;
            _container = container;
        }

        public async UniTask<Canvas> GetOrCreateByType(CanvasType type)
        {
            if (_canvases.TryGetValue(type, out Canvas canvas)) 
                return canvas;
            
            _canvases[type] = await CreateCanvas(type);;
            return _canvases[type];
        }

        public async UniTask<Canvas> GetOrCreateCurtain()
        {
            CanvasType type = CanvasType.Curtain;
            
            if (_canvases.TryGetValue(type, out Canvas canvas)) 
                return canvas;
            
            _canvases[type] = await CreateCurtain(type);;
            return _canvases[type];
        }
        
        public void ClearByType(CanvasType type)
        {
            if (_canvases.TryGetValue(type, out Canvas canvas))
            {
                if (canvas != null)
                    Object.Destroy(canvas.gameObject);

                _canvases.Remove(type);
            }
        }
        
        public void ClearAll()
        {
            foreach (Canvas canvas in _canvases.Values)
                Object.Destroy(canvas.gameObject);
    
            _canvases.Clear();
        }
        
        public void Dispose() => ClearAll();

        #region Private methods

        private async UniTask<Canvas> CreateCanvas(CanvasType type)
        {
            CanvasConfig config = _staticDataService.GetUI<CanvasConfig>(UICategory.Canvas, type);
            GameObject prefab = await _addressablesLoaderService.Load<GameObject>(config.Prefab);
            return Instantiate(prefab);
        }
        
        private async UniTask<Canvas> CreateCurtain(CanvasType type)
        {
            GameObject prefab = await _addressablesLoaderService.Load<GameObject>(key: type.ToCanvasString());
            return Instantiate(prefab);
        }

        private Canvas Instantiate(GameObject prefab)
        {
            Canvas canvas = Object.Instantiate(prefab).GetComponent<Canvas>();
            _container.InjectGameObject(canvas.gameObject);
            Object.DontDestroyOnLoad(canvas.gameObject);
            return canvas;
        }

        #endregion
    }
}