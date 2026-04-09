using Core.Enums;
using Core.Enums.Scene;
using Core.Services.SceneTransition;
using R3;

namespace Infrastructure.Services.SceneTransition
{
    public class SceneTransitionService : ISceneTransitionService
    {
        private readonly Subject<SceneType> _transitionRequested = new();

        public Observable<SceneType> OnTransitionRequested => _transitionRequested;

        public void RequestTransition(SceneType targetScene) => 
            _transitionRequested.OnNext(targetScene);
    }
}