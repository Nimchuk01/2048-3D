using Cysharp.Threading.Tasks;
using Presentation.Gameplay.Board;

namespace Core.Factories
{
    public interface IBoardFactory
    {
        UniTask<BoardEntity> CreateBoard();
    }
}
