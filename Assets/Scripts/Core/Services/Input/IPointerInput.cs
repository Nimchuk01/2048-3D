using System;
using UnityEngine;

namespace Core.Services.Input
{
    public interface IPointerInput
    {
        event Action<Vector3> PointerDown;
        event Action<Vector3> PointerMove;
        event Action<Vector3> PointerUp;
        
        bool IsDragging { get; }
        Vector3 CurrentPosition { get; }
        Vector3 StartPosition { get; }
        
        void Update();
        void Enable();
        void Disable();
    }
}