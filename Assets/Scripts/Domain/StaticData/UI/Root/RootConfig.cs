using System;
using Core.Enums.UI;
using Core.UI;
using UnityEngine.AddressableAssets;

namespace Domain.StaticData.UI.Root
{
    [Serializable]
    public class RootConfig : IUIConfig
    {
        public RootType Type;
        public AssetReference Prefab;
        
        public UIIdentifier UIIdentifier => UIIdentifier.From(Type);
    }
}