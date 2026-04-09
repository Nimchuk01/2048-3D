using Core.Enums.Scene;
using Core.Enums.UI;
using Core.Services.SceneTransition;
using Core.UI;
using Core.UI.Common;

namespace Presentation.UI.Views.Screens.Gameplay
{
    public class GameplayHUDViewModel : UIBaseViewModel
    {
        public override UIIdentifier UIIdentifier => UIIdentifier.From(ScreenType.GameplayHUD);
        
        private readonly ISceneTransitionService _sceneTransitionService;
        
        public GameplayHUDViewModel(ISceneTransitionService sceneTransitionService) => 
            _sceneTransitionService = sceneTransitionService;

        public void GoToMenu() => _sceneTransitionService.RequestTransition(SceneType.Meta);
    }
}