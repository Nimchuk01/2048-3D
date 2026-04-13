using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Domain.StaticData.Gameplay.Cubes
{
    [CreateAssetMenu(fileName = "CubeConfig", menuName = "StaticData/Gameplay/CubeConfig")]
    public class CubeStaticData : ScriptableObject
    {
        public AssetReference CubePrefab;

        [Range(0f, 1f)] public float SpawnChanceFor2 = 0.75f;
        [Range(0f, 1f)] public float SpawnChanceFor4 = 0.25f;
        
        [Min(0f)] public float MinLaunchForce = 10f;
        [Min(0f)] public float MaxLaunchForce = 15f;
        [Min(0f)] public float SpawnCooldown = 0.2f;
        [Min(0f)] public float MergeVelocityThreshold = 3f;
        [Min(0f)] public float AutoMergeCooldown = 5f;
        
        [Min(1)] public int MinCubeCount = 5;
        [Min(1)] public int MaxCubeCount = 8;
        public int[] PossibleValues = { 2, 4, 8, 16 };
    }
}
