using Core.Services.Score;
using R3;

namespace Infrastructure.Services.Score
{
    public class ScoreService : IScoreService
    {
        private readonly ReactiveProperty<int> _score = new(0);
        
        public ReadOnlyReactiveProperty<int> Score => _score;

        public void AddScore(int mergeValue)
        {
            int points = mergeValue / 2;
            _score.Value += points;
        }

        public void Reset()
        {
            _score.Value = 0;
        }
    }
}
