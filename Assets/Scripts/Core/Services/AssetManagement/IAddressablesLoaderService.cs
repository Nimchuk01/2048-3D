using System.Threading;
using Core.Enums;
using Core.Enums.Scene;
using Core.Services.Initialization;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Core.Services.AssetManagement
{
    public interface IAddressablesLoaderService : IInitializableAsync
    {
        UniTask<T> Load<T>(AssetReference assetReference, CancellationToken cancellationToken = default) where T : class;
        UniTask<T> Load<T>(string key, CancellationToken cancellationToken = default) where T : class;
        AsyncOperationHandle<SceneInstance> LoadScene(SceneType sceneType, CancellationToken cancellationToken = default);
        void Release(AssetReference assetReference);
        void Release(string key);
        void Cleanup();
    }
}