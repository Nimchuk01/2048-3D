using System.Collections.Generic;
using UnityEngine;

namespace Domain.StaticData.UI.Screen
{
    [CreateAssetMenu(fileName = "ScreenConfig", menuName = "StaticData/ScreenConfig")]
    public class ScreenStaticData : ScriptableObject
    {
        public List<ScreenConfig> Configs;
    }
}