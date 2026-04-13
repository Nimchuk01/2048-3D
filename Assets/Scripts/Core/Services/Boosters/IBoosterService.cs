using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace Core.Services.Boosters
{
    public interface IBoosterService
    {
        ReadOnlyReactiveProperty<bool> IsOnCooldown { get; }
        ReadOnlyReactiveProperty<float> RemainingCooldown { get; }
        UniTask RunAutoMerge(CancellationToken token);
    }
}
