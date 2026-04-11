using GameLogic.Gameplay.Cubes;
using UnityEngine;

namespace Core.Services.Cube
{
    public interface ICubeService
    {
        CubeEntity ActiveCube { get; }
        bool IsDragging { get; }
        
        void RegisterCube(CubeEntity cube);
        void UnregisterCube(CubeEntity cube);
        void SetActiveCube(CubeEntity cube);
        
        void StartDrag(Vector3 screenPosition);
        void Drag(Vector3 screenPosition);
        void EndDrag();
    }
}
