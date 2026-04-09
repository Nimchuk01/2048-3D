using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Enums;
using Core.Enums.Scene;
using Core.Enums.Scene.Extensions;
using Core.Logging;
using Core.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Infrastructure.Services.AssetManagement
{
    public class AddressablesLoaderService : IAddressablesLoaderService
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public async UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Addressables.InitializeAsync().Task;
            
            DebugLogger.LogMessage(message: $"Initialized", sender: this);
        }

        public async UniTask<T> Load<T>(AssetReference assetReference, CancellationToken cancellationToken) where T : class => 
            await Load<T>(key: assetReference.AssetGUID, cancellationToken: cancellationToken);

        public async UniTask<T> Load<T>(string key, CancellationToken cancellationToken) where T : class
        {
            if (_completedCache.TryGetValue(key, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            cancellationToken.ThrowIfCancellationRequested();
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);

            cancellationToken.ThrowIfCancellationRequested();
            return await RunWithCacheOnComplete(handle: handle, cacheKey: key, token: cancellationToken);
        }

        public AsyncOperationHandle<SceneInstance> LoadScene(SceneType sceneType, CancellationToken cancellationToken)
        {
            AsyncOperationHandle<SceneInstance> operationHandle = Addressables.LoadSceneAsync(key: sceneType.ToSceneString(), activateOnLoad: false);

            return operationHandle;
        }

        public void Release(AssetReference assetReference) => 
            Release(assetReference.AssetGUID);

        public void Release(string key)
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> handlesForKey))
                return;
            
            foreach (AsyncOperationHandle handle in handlesForKey)
                Addressables.Release(handle);

            _completedCache.Remove(key);
            _handles.Remove(key);
        }

        public void Cleanup()
        {
            if (_handles.Count == 0)
                return;
             
            foreach (AsyncOperationHandle handle in _handles.Values.SelectMany(resourceHandles => resourceHandles))
                Addressables.Release(handle);
      
            _completedCache.Clear();
            _handles.Clear();
        }

        #region Private methods

        private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey, CancellationToken token) where T : class
        {
            handle.Completed += completeHandle =>
            {
                _completedCache[cacheKey] = completeHandle;
            };

            token.ThrowIfCancellationRequested();
            AddHandle(cacheKey, handle);

            T result = await handle.Task;
            token.ThrowIfCancellationRequested();

            return result;
        }

        private void AddHandle(string key, AsyncOperationHandle handle)
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }
            
            resourceHandles.Add(handle);
        }

        #endregion
    }
}