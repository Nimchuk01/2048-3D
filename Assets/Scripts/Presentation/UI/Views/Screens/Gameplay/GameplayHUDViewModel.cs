using Core.Enums.UI;
using Core.Services.Boosters;
using Core.Services.Score;
using Core.UI;
using Core.UI.Common;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Presentation.UI.Views.Screens.Gameplay
{
    public class GameplayHUDViewModel : UIBaseViewModel
    {
        private const string AUTO_MERGE_LABEL = "Auto Merge";

        public override UIIdentifier UIIdentifier => UIIdentifier.From(ScreenType.GameplayHUD);
        
        private readonly IScoreService _scoreService;
        private readonly IBoosterService _boosterService;
        
        public ReadOnlyReactiveProperty<bool> IsAutoMergeOnCooldown => _boosterService.IsOnCooldown;
        public ReadOnlyReactiveProperty<int> Score => _scoreService.Score;
        public ReadOnlyReactiveProperty<float> RemainingCooldown => _boosterService.RemainingCooldown;

        public GameplayHUDViewModel(IScoreService scoreService, IBoosterService boosterService)
        {
            _scoreService = scoreService;
            _boosterService = boosterService;
        }

        public string GetScoreText(int score) => 
            $"Score: {score}";
       
        public string GetAutoMergeButtonText(float remainingSeconds) => 
            remainingSeconds > 0 ? $"{Mathf.CeilToInt(remainingSeconds)}s" : AUTO_MERGE_LABEL;

        public async UniTaskVoid ActivateAutoMerge()
        {
            await _boosterService.RunAutoMerge(default);
        }
    }
}