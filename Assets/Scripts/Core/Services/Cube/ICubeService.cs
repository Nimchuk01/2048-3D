using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.Cubes;
using UnityEngine;

namespace Core.Services.Cube
{
    public interface ICubeService
    {
        void SetCubesParent(Transform cubesParent);
        UniTask SpawnPlayerCube();
    }
}
