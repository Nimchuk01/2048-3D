using System.Threading;
using Core.Factories.UI;
using Core.Logging;
using Core.Services.Curtain;
using Cysharp.Threading.Tasks;
using Presentation.UI.Curtain;
using UnityEngine;

namespace Infrastructure.Services.Curtain
{
    public class CurtainService : ICurtainService
    {
        private CurtainView _view;
        
        private readonly ICanvasFactory _canvasFactory;

        public CurtainService(ICanvasFactory canvasFactory) => 
            _canvasFactory = canvasFactory;

        public async UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            Canvas canvas = await _canvasFactory.GetOrCreateCurtain();
            _view = canvas.GetComponent<CurtainView>();
            
            Show();
            
            DebugLogger.LogMessage(message: $"Initialized", sender: this);
        }

        public void Show(string text = "Loading...")
        {
            _view?.SetText(text);
            _view?.SetProgress(0);
            _view?.SetVisible(true);
        }

        public void Hide() => 
            _view?.SetVisible(false);

        public void SetProgress(float value) => 
            _view?.SetProgress(value);

        public void SetText(string text) => 
            _view?.SetText(text);
    }
}