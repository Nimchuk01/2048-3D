using Core.Enums.Scene;
using Core.Enums.UI;
using Core.Services.SceneTransition;
using Core.UI;
using Core.UI.Common;
using Core.UI.Management;
using Presentation.UI.Views.Popups.RewardPopup;
using Presentation.UI.Views.Popups.SettingsPopup;

namespace Presentation.UI.Views.Screens.MainMenu
{
    public class MainMenuViewModel : UIBaseViewModel
    {
        public override UIIdentifier UIIdentifier => UIIdentifier.From(ScreenType.MainMenu);
        
        private readonly UIManager _uiManager;
        private readonly ISceneTransitionService _sceneTransitionService;
        
        public MainMenuViewModel(UIManager uiManager, ISceneTransitionService sceneTransitionService)
        {
            _uiManager = uiManager;
            _sceneTransitionService = sceneTransitionService;
        }

        public void OpenSettings() => _uiManager.OpenPopup<SettingsPopupViewModel>();
        public void OpenRewards() => _uiManager.OpenPopup<RewardPopupViewModel>();
        public void GoToGame() => _sceneTransitionService.RequestTransition(SceneType.Core);
    }
}