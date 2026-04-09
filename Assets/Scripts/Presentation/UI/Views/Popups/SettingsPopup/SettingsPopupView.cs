using Core.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Views.Popups.SettingsPopup
{
    public class SettingsPopupView : UIView<SettingsPopupViewModel>
    {
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _closeButton;

        protected override void OnBind(SettingsPopupViewModel vm)
        {
            _applyButton?.onClick.AddListener(vm.ApplySettings);
            _closeButton?.onClick.AddListener(vm.RequestClose);
        }

        private void OnDestroy()
        {
            _applyButton?.onClick.RemoveAllListeners();
            _closeButton?.onClick.RemoveAllListeners();
        }
    }
}