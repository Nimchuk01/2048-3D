using Core.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Views.Popups.RewardPopup
{
    public class RewardPopupView : UIView<RewardPopupViewModel>
    {
        [SerializeField] private Button _claimButton;
        [SerializeField] private Button _closeButton;

        protected override void OnBind(RewardPopupViewModel vm)
        {
            _claimButton?.onClick.AddListener(vm.ClaimReward);
            _closeButton?.onClick.AddListener(vm.RequestClose);
        }

        private void OnDestroy()
        {
            _claimButton?.onClick.RemoveAllListeners();
            _closeButton?.onClick.RemoveAllListeners();
        }
    }
}