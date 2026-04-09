using System;
using Core.Enums.UI;
using Core.UI;
using UnityEngine.AddressableAssets;

namespace Domain.StaticData.UI.Screen
{
    [Serializable]
    public class ScreenConfig : IUIConfig
    {
        public ScreenType Type;
        public AssetReference Prefab;
        
        public UIIdentifier UIIdentifier => UIIdentifier.From(Type);
    }
}