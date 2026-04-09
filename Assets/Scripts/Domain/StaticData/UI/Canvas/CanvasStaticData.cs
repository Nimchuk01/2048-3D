using System.Collections.Generic;
using UnityEngine;

namespace Domain.StaticData.UI.Canvas
{
    [CreateAssetMenu(fileName = "CanvasConfig", menuName = "StaticData/CanvasConfig")]
    public class CanvasStaticData : ScriptableObject
    {
        public List<CanvasConfig> Configs;
    }
}