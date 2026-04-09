using Core.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Views.Screens.MainMenu
{
    public class MainMenuView : UIView<MainMenuViewModel>
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _rewardsButton;
        [SerializeField] private Button _playButton;

        protected override void OnBind(MainMenuViewModel vm)
        {
            _settingsButton.onClick.AddListener(vm.OpenSettings);
            _rewardsButton.onClick.AddListener(vm.OpenRewards);
            _playButton.onClick.AddListener(vm.GoToGame);
        }

        private void OnDestroy()
        {
            _settingsButton.onClick.RemoveAllListeners();
            _rewardsButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
        }
    }
}