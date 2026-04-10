using UnityEngine;

namespace Domain.StaticData.Gameplay.Boosters
{
    [CreateAssetMenu(fileName = "AutoMergeBoosterConfig", menuName = "StaticData/Gameplay/AutoMergeBoosterConfig")]
    public class AutoMergeBoosterStaticData : ScriptableObject
    {
        [Min(0f)] public float RiseDuration = 0.3f;
        [Min(0f)] public float SwingDuration = 0.15f;
        [Min(0f)] public float MergeDuration = 0.25f;
        
        [Min(0f)] public float RiseHeight = 2f;
        [Min(0f)] public float SwingBackDistance = 0.35f;
    }
}
