using Core.UI.Common;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Views.Screens.Gameplay
{
    public class GameplayHUDView : UIView<GameplayHUDViewModel>
    {
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private Button _autoMergeButton;
        [SerializeField] private TMP_Text _autoMergeCooldownText;

        private readonly CompositeDisposable _disposables = new();
        
        protected override void OnBind(GameplayHUDViewModel vm)
        {
            vm.Score.Subscribe(score => _scoreLabel.text = vm.GetScoreText(score)).AddTo(_disposables);
            vm.RemainingCooldown.Subscribe(remaining => _autoMergeCooldownText.text = vm.GetAutoMergeButtonText(remaining)).AddTo(_disposables);
            vm.IsAutoMergeOnCooldown.Subscribe(isOnCooldown => _autoMergeButton.interactable = !isOnCooldown).AddTo(_disposables);
            
            _autoMergeButton.onClick.AddListener(() => vm.ActivateAutoMerge().Forget());
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
            _autoMergeButton.onClick.RemoveAllListeners();
        }
    }
}