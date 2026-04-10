using System;
using Core.Enums.UI;
using Core.Services.Initialization;
using Domain.StaticData.Gameplay.Boosters;
using Domain.StaticData.Gameplay.Cubes;
using Domain.StaticData.Gameplay.Input;
using Domain.StaticData.Gameplay.Merge;
using Domain.StaticData.UI;

namespace Core.Services.StaticData
{
    public interface IStaticDataService : IInitializableAsync
    {
        T GetUI<T>(UICategory category, Enum typeEnum) where T : class, IUIConfig;
        CubeStaticData CubeConfig { get; }
        InputStaticData InputConfig { get; }
        MergeStaticData MergeConfig { get; }
        AutoMergeBoosterStaticData AutoMergeBoosterConfig { get; }
    }
}