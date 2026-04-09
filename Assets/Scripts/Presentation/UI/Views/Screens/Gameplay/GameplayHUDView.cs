using Core.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Views.Screens.Gameplay
{
    public class GameplayHUDView : UIView<GameplayHUDViewModel>
    {
        [SerializeField] private Button _menuButton;
        
        protected override void OnBind(GameplayHUDViewModel vm) => 
            _menuButton.onClick.AddListener(vm.GoToMenu);

        private void OnDestroy() => 
            _menuButton.onClick.RemoveAllListeners();
    }
}