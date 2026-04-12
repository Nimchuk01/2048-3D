using System;
using Core.Services.GameOver;
using Core.UI.Management;
using Presentation.UI.Views.Popups.GameOver;
using R3;
using Zenject;

namespace Infrastructure.Services.GameOver
{
    public class GameOverWatcher : IInitializable, IDisposable
    {
        private readonly IGameOverService _gameOverService;
        private readonly UIManager _uiManager;
        private readonly CompositeDisposable _disposables = new();

        public GameOverWatcher(IGameOverService gameOverService, UIManager uiManager)
        {
            _gameOverService = gameOverService;
            _uiManager = uiManager;
        }

        public void Initialize()
        {
            _gameOverService.IsGameOver.Subscribe(isOver =>
            {
                if (isOver) ShowGameOver();
            }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void ShowGameOver()
        {
            _uiManager.OpenPopup<GameOverViewModel>();
        }
    }
}
