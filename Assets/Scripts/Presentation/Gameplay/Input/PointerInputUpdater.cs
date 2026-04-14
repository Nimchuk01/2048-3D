using Core.Services.Input;
using UnityEngine;
using Zenject;

namespace Presentation.Gameplay.Input
{
    public class PointerInputUpdater : MonoBehaviour
    {
        private IPointerInput _inputService;

        [Inject]
        public void Construct(IPointerInput inputService)
        {
            _inputService = inputService;
        }

        private void Update()
        {
            _inputService?.Update();
        }
    }
}
