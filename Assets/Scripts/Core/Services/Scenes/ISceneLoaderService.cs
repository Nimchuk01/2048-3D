using System;
using System.Threading;
using Core.Enums;
using Core.Enums.Scene;
using Cysharp.Threading.Tasks;

namespace Core.Services.Scenes
{
    public interface ISceneLoaderService
    {
        UniTask LoadSceneAsync(SceneType sceneType, Func<UniTask> postLoadLogic = null, CancellationToken cancellationToken = default);
    }
}