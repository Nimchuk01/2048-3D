using Core.Logging;
using GameLogic.GameLoop.Core;
using GameLogic.GameLoop.States;
using Zenject;

namespace Infrastructure.Installers
{
    public class StateMachineInstaller : Installer<StateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
            
            BindStates();
            
            DebugLogger.LogMessage(message: $"Installed", sender: this);
        }

        private void BindStates()
        {
            Container.Bind<MetaState>().AsSingle();
            Container.Bind<CoreState>().AsSingle();
        }
    }
}
