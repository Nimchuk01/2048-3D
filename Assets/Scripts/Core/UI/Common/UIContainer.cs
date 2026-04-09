using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Factories.UI;
using Core.UI.Common.Interfaces;
using UnityEngine;
using Zenject;

namespace Core.UI.Common
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private Transform _screensContainer;
        [SerializeField] private Transform _popupsContainer;

        private readonly Dictionary<UIBaseViewModel, IUIView> _openedPopupViews = new();
        private IUIView _openedScreenView;

        private IUIFactory _iuiFactory;

        [Inject]
        private void Construct(IUIFactory iuiFactory) => 
            _iuiFactory = iuiFactory;

        public async void OpenPopup(UIBaseViewModel viewModel)
        {
            IUIView view = await CreateUI(viewModel, _popupsContainer);
            view.Bind(viewModel);
            _openedPopupViews.Add(viewModel, view);
        }

        public void ClosePopup(UIBaseViewModel viewModel)
        {
            if (!_openedPopupViews.TryGetValue(viewModel, out IUIView view)) return;
            view.Close();
            _openedPopupViews.Remove(viewModel);
        }

        public async void OpenScreen(UIBaseViewModel viewModel)
        {
            _openedScreenView?.Close();
            _openedScreenView = await CreateUI(viewModel, _screensContainer);
            _openedScreenView.Bind(viewModel);
        }

        private async Task<IUIView> CreateUI(UIBaseViewModel viewModel, Transform parent)
        {
            GameObject go = await _iuiFactory.Create(viewModel.UIIdentifier, parent);
            return go.GetComponent<IUIView>();
        }
    }
}