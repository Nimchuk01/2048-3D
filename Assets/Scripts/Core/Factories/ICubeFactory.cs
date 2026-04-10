using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.Cubes;
using UnityEngine;

namespace Core.Factories
{
    public interface ICubeFactory
    {
        UniTask<CubeEntity> CreateCube(Transform parent);
    }
}
