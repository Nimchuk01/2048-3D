using UnityEngine;

namespace Domain.StaticData.Gameplay.Merge
{
    [CreateAssetMenu(fileName = "MergeConfig", menuName = "StaticData/Gameplay/MergeConfig")]
    public class MergeStaticData : ScriptableObject
    {
        [Min(0f)] public float MinImpulseToMerge = 1.5f;
        [Range(-1f, 1f)] public float MinDirectionalDot = 0.35f;
    }
}
