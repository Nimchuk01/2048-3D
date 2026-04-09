using Core.UI.Common;
using Core.UI.Root;
using ObservableCollections;
using R3;
using UnityEngine;
using Zenject;

namespace Presentation.UI.Views.Root
{
    public class OverlayRootView : MonoBehaviour
    {
        [SerializeField] private UIContainer _container;

        private readonly CompositeDisposable _subscriptions = new();

        [Inject]
        private void Construct(OverlayRootViewModel rootViewModel) => 
            Bind(rootViewModel);

        private void OnDestroy() => _subscriptions.Dispose();

        private void Bind(OverlayRootViewModel vm)
        {
            _subscriptions.Add(vm.ActiveScreen.Subscribe(screen =>
            {
                if (screen != null) _container.OpenScreen(screen);
            }));

            foreach (UIBaseViewModel popup in vm.OpenedPopups)
                _container.OpenPopup(popup);

            _subscriptions.Add(vm.OpenedPopups.ObserveAdd().Subscribe(e =>
            {
                _container.OpenPopup(e.Value);
            }));
            
            _subscriptions.Add(vm.OpenedPopups.ObserveRemove().Subscribe(e =>
            {
                _container.ClosePopup(e.Value);
            }));
        }
    }
}