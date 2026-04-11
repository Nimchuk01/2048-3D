using System;
using Core.Services.Input;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Infrastructure.Services.Input
{
    public class TouchPointerInput : IPointerInput
    {
        public event Action<Vector3> PointerDown;
        public event Action<Vector3> PointerMove;
        public event Action<Vector3> PointerUp;

        public bool IsDragging { get; private set; }
        public Vector3 CurrentPosition { get; private set; }
        public Vector3 StartPosition { get; private set; }

        private bool _enabled = true;

        public void Enable() => _enabled = true;
        public void Disable() => _enabled = false;

        public void Update()
        {
            if (!_enabled) return;
            if (UnityInput.touchCount == 0) return;

            Touch touch = UnityInput.GetTouch(0);
            Vector3 position = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    IsDragging = true;
                    StartPosition = position;
                    CurrentPosition = position;
                    PointerDown?.Invoke(position);
                    break;

                case TouchPhase.Moved:
                    CurrentPosition = position;
                    PointerMove?.Invoke(position);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    IsDragging = false;
                    PointerUp?.Invoke(position);
                    break;
            }
        }
    }
}