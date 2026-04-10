using System;
using System.Collections.Generic;
using System.Threading;
using Core.Enums.UI;
using Core.Logging;
using Core.Services.AssetManagement;
using Core.Services.StaticData;
using Core.UI;
using Cysharp.Threading.Tasks;
using Domain.StaticData.Gameplay.Cubes;
using Domain.StaticData.UI;
using Domain.StaticData.UI.Canvas;
using Domain.StaticData.UI.Popup;
using Domain.StaticData.UI.Root;
using Domain.StaticData.UI.Screen;

namespace Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly Dictionary<UIIdentifier, IUIConfig> _uiConfigs = new();
        
        private readonly IAddressablesLoaderService _addressablesLoaderService;

        public CubeStaticData CubeConfig { get; private set; }
        
        public StaticDataService(IAddressablesLoaderService addressablesLoaderService) => 
            _addressablesLoaderService = addressablesLoaderService;

        public async UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            await InitializeUIConfigs();
            await InitializeGameplayConfigs();
            
            DebugLogger.LogMessage(message: $"Loaded", sender: this);
        }
        
        public T GetUI<T>(UICategory category, Enum typeEnum) where T : class, IUIConfig
        {
            UIIdentifier id = new UIIdentifier(category, Convert.ToInt32(typeEnum));

            if (_uiConfigs.TryGetValue(id, out IUIConfig config) && config is T casted)
                return casted;

            throw new Exception($"UI config not found for category {category} and type {typeEnum}");
        }

        #region Initialize UI

        private async UniTask InitializeUIConfigs()
        {
            await LoadCanvasConfigs();
            await LoadRootConfigs();
            await LoadScreenConfigs();
            await LoadPopupConfigs();
        }

        private async UniTask LoadCanvasConfigs()
        {
            CanvasStaticData data = await _addressablesLoaderService.Load<CanvasStaticData>(key: StaticDataPaths.CANVAS_CONFIG);
            foreach (CanvasConfig cfg in data.Configs)
                _uiConfigs[cfg.UIIdentifier] = cfg;
        }

        private async UniTask LoadRootConfigs()
        {
            RootStaticData data = await _addressablesLoaderService.Load<RootStaticData>(key: StaticDataPaths.ROOT_CONFIG);
            foreach (RootConfig cfg in data.Configs)
                _uiConfigs[cfg.UIIdentifier] = cfg;
        }

        private async UniTask LoadScreenConfigs()
        {
            ScreenStaticData data = await _addressablesLoaderService.Load<ScreenStaticData>(key: StaticDataPaths.SCREEN_CONFIG);
            foreach (ScreenConfig cfg in data.Configs)
                _uiConfigs[cfg.UIIdentifier] = cfg;
        }

        private async UniTask LoadPopupConfigs()
        {
            PopupStaticData data = await _addressablesLoaderService.Load<PopupStaticData>(key: StaticDataPaths.POPUP_CONFIG);
            foreach (PopupConfig cfg in data.Configs)
                _uiConfigs[cfg.UIIdentifier] = cfg;
        }

        #endregion

        #region Initialize Gameplay

        private async UniTask InitializeGameplayConfigs()
        {
            CubeConfig = await _addressablesLoaderService.Load<CubeStaticData>(key: StaticDataPaths.CUBE_CONFIG);
        }

        #endregion
    }
}