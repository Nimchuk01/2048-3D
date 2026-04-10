using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Factories
{
    public interface IBoardFactory
    {
        UniTask<GameObject> CreateBoard(Transform parent = null);
    }
}
