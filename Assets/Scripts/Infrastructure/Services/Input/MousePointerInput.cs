using System;
using Core.Services.Input;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Infrastructure.Services.Input
{
    public class MousePointerInput : IPointerInput
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

            if (UnityInput.GetMouseButtonDown(0))
            {
                IsDragging = true;
                StartPosition = UnityInput.mousePosition;
                CurrentPosition = UnityInput.mousePosition;
                PointerDown?.Invoke(UnityInput.mousePosition);
            }
            else if (UnityInput.GetMouseButton(0) && IsDragging)
            {
                CurrentPosition = UnityInput.mousePosition;
                PointerMove?.Invoke(UnityInput.mousePosition);
            }
            else if (UnityInput.GetMouseButtonUp(0) && IsDragging)
            {
                IsDragging = false;
                PointerUp?.Invoke(UnityInput.mousePosition);
            }
        }
    }
}