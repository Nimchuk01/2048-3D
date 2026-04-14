using System.Threading;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Timing
{
    public static class Wait
    {
        public static UniTask Seconds(float seconds, CancellationToken token = default)
        {
            return UniTask.Delay((int)(seconds * 1000), cancellationToken: token);
        }

        public static UniTask Milliseconds(int milliseconds, CancellationToken token = default)
        {
            return UniTask.Delay(milliseconds, cancellationToken: token);
        }
    }
}
