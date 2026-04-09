using System;
using Core.Enums.UI;
using Core.Services.Initialization;
using Core.UI;
using Domain.StaticData.UI;

namespace Core.Services.StaticData
{
    public interface IStaticDataService : IInitializableAsync
    {
        T GetUI<T>(UICategory category, Enum typeEnum) where T : class, IUIConfig;
    }
}