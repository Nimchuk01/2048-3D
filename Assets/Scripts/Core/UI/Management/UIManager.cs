using System.Threading;
using Core.Enums.UI;
using Core.Factories.UI;
using Core.Services.Initialization;
using Core.UI.Common;
using Core.UI.Root;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.UI.Management
{
    public class UIManager : IInitializableAsync
    {
        private readonly DiContainer _container;
        private readonly OverlayRootViewModel _rootViewModel;
        private readonly ICanvasFactory _canvasFactory;
        private readonly IUIFactory _iuiFactory;

        public UIManager(DiContainer container, OverlayRootViewModel rootViewModel, ICanvasFactory canvasFactory, IUIFactory iuiFactory)
        {
            _container = container;
            _rootViewModel = rootViewModel;
            _canvasFactory = canvasFactory;
            _iuiFactory = iuiFactory;
        }
        
        public async UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            Canvas overlayCanvas = await _canvasFactory.GetOrCreateByType(CanvasType.Overlay);
            await _iuiFactory.Create(RootType.OverlayRoot, overlayCanvas.transform);
        }

        public T OpenScreen<T>() where T : UIBaseViewModel
        {
            T viewModel = CreateViewModel<T>();
            _rootViewModel.OpenScreen(viewModel);
            return viewModel;
        }

        public T OpenPopup<T>() where T : UIBaseViewModel
        {
            T viewModel = CreateViewModel<T>();
            _rootViewModel.OpenPopup(viewModel);
            return viewModel;
        }

        private T CreateViewModel<T>() where T : UIBaseViewModel =>
            _container.Resolve<T>();
    }
}