using System;
using System.Collections.Generic;
using System.Linq;
using Core.UI.Common;
using ObservableCollections;
using R3;

namespace Core.UI.Root
{
    public class OverlayRootViewModel : IDisposable
    {
        public ReadOnlyReactiveProperty<UIBaseViewModel> ActiveScreen => _activeScreen;
        public IObservableCollection<UIBaseViewModel> OpenedPopups => _openedPopups;

        private readonly ReactiveProperty<UIBaseViewModel> _activeScreen = new(null);
        private readonly ObservableList<UIBaseViewModel> _openedPopups = new();
        private readonly Dictionary<UIBaseViewModel, IDisposable> _popupSubscriptions = new();

        public void OpenScreen(UIBaseViewModel viewModel)
        {
            _activeScreen.Value?.Dispose();
            _activeScreen.Value = viewModel;
        }

        public void OpenPopup(UIBaseViewModel viewModel)
        {
            if (_openedPopups.Contains(viewModel))
                return;

            IDisposable subscription = viewModel.CloseRequested.Subscribe(ClosePopup);
            _popupSubscriptions[viewModel] = subscription;
            _openedPopups.Add(viewModel);
        }

        public void ClosePopup(UIBaseViewModel viewModel)
        {
            if (!_openedPopups.Contains(viewModel)) return;

            viewModel.Dispose();
            _openedPopups.Remove(viewModel);
            _popupSubscriptions[viewModel]?.Dispose();
            _popupSubscriptions.Remove(viewModel);
        }

        public void ClosePopup(UIIdentifier identifier)
        {
            UIBaseViewModel viewModel = _openedPopups.FirstOrDefault(viewModel => viewModel.UIIdentifier.Equals(identifier));
            if (viewModel != null) ClosePopup(viewModel);
        }

        public void CloseAllPopups()
        {
            foreach (UIBaseViewModel popup in _openedPopups.ToArray())
                ClosePopup(popup);
        }

        public void Dispose() => CloseAllPopups();
    }
}