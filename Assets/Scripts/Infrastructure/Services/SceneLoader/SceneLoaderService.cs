using System;
using System.Threading;
using Core.Enums;
using Core.Enums.Scene;
using Core.Services.AssetManagement;
using Core.Services.Curtain;
using Core.Services.Scenes;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Infrastructure.Services.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private const float SCENE_LOADING_PROGRESS_WEIGHT = 0.6f;
        private const int CURTAIN_CLOSE_DELAY = 300;
        
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly ICurtainService _curtainService;

        public SceneLoaderService(IAddressablesLoaderService addressablesLoaderService, ICurtainService curtainService)
        {
            _addressablesLoaderService = addressablesLoaderService;
            _curtainService = curtainService;
        }
        
        public async UniTask LoadSceneAsync(SceneType sceneType, Func<UniTask> postLoadLogic = null, CancellationToken cancellationToken = default)
        {
            _curtainService.Show(text: $"Loading {sceneType}...");

            AsyncOperationHandle<SceneInstance> sceneHandle = _addressablesLoaderService.LoadScene(sceneType, cancellationToken);
            while (!sceneHandle.IsDone)
            {
                _curtainService.SetProgress(sceneHandle.PercentComplete * SCENE_LOADING_PROGRESS_WEIGHT);
                await UniTask.Yield();
            }

            await sceneHandle.Task;
            await sceneHandle.Result.ActivateAsync();

            if (postLoadLogic != null) 
                await postLoadLogic.Invoke();

            _curtainService.SetProgress(1f);
            await UniTask.Delay(millisecondsDelay: CURTAIN_CLOSE_DELAY, cancellationToken: cancellationToken);

            _curtainService.Hide();
        }

    }
}