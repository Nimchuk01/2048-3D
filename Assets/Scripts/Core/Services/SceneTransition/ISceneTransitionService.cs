using Core.Enums;
using Core.Enums.Scene;
using R3;

namespace Core.Services.SceneTransition
{
    public interface ISceneTransitionService
    {
        Observable<SceneType> OnTransitionRequested { get; }
        
        void RequestTransition(SceneType targetScene);
    }
}