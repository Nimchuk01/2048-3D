using System;
using Core.Enums.UI;
using Core.UI;
using UnityEngine.AddressableAssets;

namespace Domain.StaticData.UI.Popup
{
    [Serializable]
    public class PopupConfig : IUIConfig
    {
        public PopupType Type;
        public AssetReference Prefab;
        
        public UIIdentifier UIIdentifier => UIIdentifier.From(Type);
    }
}