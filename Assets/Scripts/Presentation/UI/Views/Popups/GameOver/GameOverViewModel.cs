using Core.Enums.UI;
using Core.Services.Score;
using Core.UI;
using Core.UI.Common;
using R3;

namespace Presentation.UI.Views.Popups.GameOver
{
    public class GameOverViewModel : UIBaseViewModel
    {
        public override UIIdentifier UIIdentifier => UIIdentifier.From(PopupType.GameOver);
        
        private readonly IScoreService _scoreService;

        public ReadOnlyReactiveProperty<int> Score => _scoreService.Score;

        public GameOverViewModel(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }
    }
}
