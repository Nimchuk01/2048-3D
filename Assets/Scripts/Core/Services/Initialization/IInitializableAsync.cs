using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Services.Initialization
{
    public interface IInitializableAsync
    {
        UniTask InitializeAsync(CancellationToken cancellationToken = default);
    }
}