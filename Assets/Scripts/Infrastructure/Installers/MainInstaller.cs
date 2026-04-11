using Core.Factories;
using Core.Factories.UI;
using Core.Services.Board;
using Core.Services.Cube;
using Core.Services.Input;
using Core.Logging;
using Core.Services.Progress;
using Core.Services.Scenes;
using Core.Services.SceneTransition;
using Infrastructure.EntryPoint;
using Infrastructure.Factories;
using Infrastructure.Factories.UI;
using Infrastructure.Input;
using Infrastructure.Services.AssetManagement;
using Infrastructure.Services.Board;
using Infrastructure.Services.Cube;
using Infrastructure.Services.Curtain;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.SceneTransition;
using Infrastructure.Services.StaticData;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SetMaxFPSGame();
            
            BindServices();
            BindFactories();

            StateMachineInstaller.Install(Container);
            UIManagerInstaller.Install(Container);
            MediatorInstaller.Install(Container);

            DebugLogger.LogMessage(message: $"Installed", sender: this);
            
            Container.BindInterfacesAndSelfTo<GameBootstrapper>().AsSingle().NonLazy(); // Bootstrapper
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<CurtainService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddressablesLoaderService>().AsSingle();
            Container.Bind<IProgressService>().To<ProgressService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProgressStorage>().AsSingle();
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();
            Container.Bind<ISceneTransitionService>().To<SceneTransitionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneTransitionWatcher>().AsSingle();
            Container.Bind<IBoardService>().To<BoardService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CubeService>().AsSingle().NonLazy();
            
            BindPointerInput();
        }

        private void BindPointerInput()
        {
            if (Application.isMobilePlatform)
                Container.Bind<IPointerInput>().To<TouchPointerInput>().AsSingle();
            else
                Container.Bind<IPointerInput>().To<MousePointerInput>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IObjectPoolFactory>().To<ObjectPoolFactory>().AsSingle();
            Container.Bind<ICanvasFactory>().To<CanvasFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<IBoardFactory>().To<BoardFactory>().AsSingle();
            Container.Bind<ICubeFactory>().To<CubeFactory>().AsSingle();
        }

        private static void SetMaxFPSGame() => 
            Application.targetFrameRate = 90;
    }
}