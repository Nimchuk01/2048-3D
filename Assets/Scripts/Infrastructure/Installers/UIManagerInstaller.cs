using Core.Logging;
using Core.UI.Management;
using Core.UI.Root;
using Presentation.UI.Views.Popups.GameOver;
using Presentation.UI.Views.Screens.Gameplay;
using Zenject;

namespace Infrastructure.Installers
{
    public class UIManagerInstaller : Installer<UIManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();
            
            BindViewModels();
            
            DebugLogger.LogMessage(message: $"Installed", sender: this);
        }

        private void BindViewModels()
        {
            Container.Bind<OverlayRootViewModel>().AsSingle();
            Container.Bind<GameplayHUDViewModel>().AsTransient();
            Container.Bind<GameOverViewModel>().AsTransient();
        }
    }
}