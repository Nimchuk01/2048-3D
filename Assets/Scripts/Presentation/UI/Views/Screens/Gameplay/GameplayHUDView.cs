using Core.UI.Common;
using R3;
using TMPro;
using UnityEngine;

namespace Presentation.UI.Views.Screens.Gameplay
{
    public class GameplayHUDView : UIView<GameplayHUDViewModel>
    {
        [SerializeField] private TMP_Text _scoreLabel;

        private readonly CompositeDisposable _disposables = new();
        
        protected override void OnBind(GameplayHUDViewModel vm) => 
            vm.Score.Subscribe(UpdateScore).AddTo(_disposables);

        private void UpdateScore(int score) => 
            _scoreLabel.text = $"Score: {score}";

        private void OnDestroy() => 
            _disposables.Dispose();
    }
}