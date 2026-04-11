using Cysharp.Threading.Tasks;
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
        
        void StartDrag(Vector3 screenPosition);
        void Drag(Vector3 screenPosition);
        void EndDrag();
        void SetCubesParent(Transform cubesParent);
        UniTask SpawnPlayerCube();
    }
}
