using System;
using Core.Enums.UI;
using Core.Services.Initialization;
using Domain.StaticData.Gameplay.Cubes;
using Domain.StaticData.UI;

namespace Core.Services.StaticData
{
    public interface IStaticDataService : IInitializableAsync
    {
        T GetUI<T>(UICategory category, Enum typeEnum) where T : class, IUIConfig;
        CubeStaticData CubeConfig { get; }
    }
}