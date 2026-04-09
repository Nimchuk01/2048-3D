using Core.Services.Initialization;
using GameLogic.GameLoop.States.Interfaces;

namespace GameLogic.GameLoop.Core.Interfaces
{
    public interface IGameStateMachine : IInitializableAsync
    {
        void Enter<TState>() where TState : class, IState;
    }
}