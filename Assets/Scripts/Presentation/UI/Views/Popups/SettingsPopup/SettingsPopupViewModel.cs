using Core.Enums.UI;
using Core.UI;
using Core.UI.Common;

namespace Presentation.UI.Views.Popups.SettingsPopup
{
    public class SettingsPopupViewModel : UIBaseViewModel
    {
        public override UIIdentifier UIIdentifier => UIIdentifier.From(PopupType.SettingsPopup);

        public void ApplySettings()
        {
            RequestClose();
        }
    }
}