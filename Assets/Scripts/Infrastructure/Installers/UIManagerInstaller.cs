using Core.Logging;
using Core.UI.Management;
using Core.UI.Root;
using Presentation.UI.Views.Popups.RewardPopup;
using Presentation.UI.Views.Popups.SettingsPopup;
using Presentation.UI.Views.Root;
using Presentation.UI.Views.Screens.Gameplay;
using Presentation.UI.Views.Screens.MainMenu;
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
            Container.Bind<MainMenuViewModel>().AsTransient();
            Container.Bind<GameplayHUDViewModel>().AsTransient();
            Container.Bind<SettingsPopupViewModel>().AsTransient();
            Container.Bind<RewardPopupViewModel>().AsTransient();
        }
    }
}