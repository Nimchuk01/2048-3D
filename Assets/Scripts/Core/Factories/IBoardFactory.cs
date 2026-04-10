using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.Board;
using UnityEngine;

namespace Core.Factories
{
    public interface IBoardFactory
    {
        UniTask<BoardEntity> CreateBoard();
    }
}
