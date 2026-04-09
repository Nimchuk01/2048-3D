using System.Collections.Generic;
using UnityEngine;

namespace Domain.StaticData.UI.Popup
{
    [CreateAssetMenu(fileName = "PopupConfig", menuName = "StaticData/PopupConfig")]
    public class PopupStaticData : ScriptableObject
    {
        public List<PopupConfig> Configs;
    }
}