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
    }
}
