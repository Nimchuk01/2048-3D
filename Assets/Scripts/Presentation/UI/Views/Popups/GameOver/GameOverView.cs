using Core.UI.Common;
using TMPro;
using UnityEngine;

namespace Presentation.UI.Views.Popups.GameOver
{
    public class GameOverView : UIView<GameOverViewModel>
    {
        [SerializeField] private TMP_Text _scoreLabel;

        protected override void OnBind(GameOverViewModel vm)
        {
            _scoreLabel.text = $"Total score: {vm.Score.CurrentValue}";
        }
    }
}
