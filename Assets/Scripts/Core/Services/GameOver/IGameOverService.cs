using R3;

namespace Core.Services.GameOver
{
    public interface IGameOverService
    {
        ReadOnlyReactiveProperty<bool> IsGameOver { get; }
        void RegisterCube();
        void UnregisterCube();
    }
}
