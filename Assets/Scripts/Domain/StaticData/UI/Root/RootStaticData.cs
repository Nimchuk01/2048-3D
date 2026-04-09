using System.Collections.Generic;
using UnityEngine;

namespace Domain.StaticData.UI.Root
{
    [CreateAssetMenu(fileName = "RootConfig", menuName = "StaticData/RootConfig")]
    public class RootStaticData : ScriptableObject
    {
        public List<RootConfig> Configs;
    }
}