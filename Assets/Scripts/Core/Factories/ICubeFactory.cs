using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.Cubes;
using UnityEngine;

namespace Core.Factories
{
    public interface ICubeFactory
    {
        UniTask<CubeEntity> CreateCube(Transform parent, bool isBoardCube = false, int? value = null);
        void ReturnCube(CubeEntity cube);
        UniTask Preload(Transform parent, int count = 20);
    }
}
