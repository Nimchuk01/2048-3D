using Core.Enums.UI;
using Core.UI;
using Core.UI.Common;

namespace Presentation.UI.Views.Popups.RewardPopup
{
    public class RewardPopupViewModel : UIBaseViewModel
    {
        public override UIIdentifier UIIdentifier => UIIdentifier.From(PopupType.RewardPopup);
        
        public void ClaimReward()
        {
            RequestClose();
        }
    }
}