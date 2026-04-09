using System;
using Core.Enums.UI;
using Core.UI;
using UnityEngine.AddressableAssets;

namespace Domain.StaticData.UI.Canvas
{
    [Serializable]
    public class CanvasConfig : IUIConfig
    {
        public CanvasType Type;
        public AssetReference Prefab;
        
        public UIIdentifier UIIdentifier => UIIdentifier.From(Type);
    }
}