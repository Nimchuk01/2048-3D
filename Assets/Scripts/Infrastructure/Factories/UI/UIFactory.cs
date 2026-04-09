using System;
using Core.Enums.UI;
using Core.Factories.UI;
using Core.Services.AssetManagement;
using Core.Services.StaticData;
using Core.UI;
using Cysharp.Threading.Tasks;
using Domain.StaticData.UI.Popup;
using Domain.StaticData.UI.Root;
using Domain.StaticData.UI.Screen;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories.UI
{
    public class UIFactory : IUIFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly DiContainer _container;

        public UIFactory(IStaticDataService staticDataService, IAddressablesLoaderService addressablesLoaderService, 
            DiContainer container)
        {
            _staticDataService = staticDataService;
            _addressablesLoaderService = addressablesLoaderService;
            _container = container;
        }
        
        public async UniTask<GameObject> Create(UIIdentifier identifier, Transform parent)
        {
            if (identifier.Category == UICategory.Canvas)
                throw new InvalidOperationException("Use CanvasFactory to create canvases!");

            GameObject prefab = await LoadPrefab(identifier);
            GameObject go = Object.Instantiate(prefab, parent);
            
            _container.InjectGameObject(go);
            
            return go;
        }
        
        public async UniTask<GameObject> Create(Enum typeEnum, Transform parent)
        {
            if (typeEnum == null)
                throw new ArgumentNullException(nameof(typeEnum));

            UICategory category = GetCategoryFromEnum(typeEnum);
            UIIdentifier id = new UIIdentifier(category, Convert.ToInt32(typeEnum));

            return await Create(id, parent);
        }

        #region Private methods

        private async UniTask<GameObject> LoadPrefab(UIIdentifier identifier)
        {
            switch (identifier.Category)
            {
                case UICategory.Popup:
                    PopupConfig popupCfg = _staticDataService.GetUI<PopupConfig>(identifier.Category, (Enum)Enum.ToObject(typeof(PopupType), identifier.Id));
                    return await _addressablesLoaderService.Load<GameObject>(popupCfg.Prefab);

                case UICategory.Screen:
                    ScreenConfig screenCfg = _staticDataService.GetUI<ScreenConfig>(identifier.Category, (Enum)Enum.ToObject(typeof(ScreenType), identifier.Id));
                    return await _addressablesLoaderService.Load<GameObject>(screenCfg.Prefab);

                case UICategory.Root:
                    RootConfig rootCfg = _staticDataService.GetUI<RootConfig>(identifier.Category, (Enum)Enum.ToObject(typeof(RootType), identifier.Id));
                    return await _addressablesLoaderService.Load<GameObject>(rootCfg.Prefab);

                default:
                    throw new ArgumentOutOfRangeException(nameof(identifier.Category), $"Unsupported category: {identifier.Category}");
            }
        }
        
        private static UICategory GetCategoryFromEnum(Enum enumValue)
        {
            Type type = enumValue.GetType();

            if (type == typeof(ScreenType))
                return UICategory.Screen;
            if (type == typeof(PopupType))
                return UICategory.Popup;
            if (type == typeof(RootType))
                return UICategory.Root;

            throw new ArgumentException($"Unsupported UI enum type: {type.Name}");
        }

        #endregion
    }
}