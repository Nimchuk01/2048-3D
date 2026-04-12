using R3;

namespace Core.Services.Score
{
    public interface IScoreService
    {
        ReadOnlyReactiveProperty<int> Score { get; }
        void AddScore(int mergeValue);
        void Reset();
    }
}