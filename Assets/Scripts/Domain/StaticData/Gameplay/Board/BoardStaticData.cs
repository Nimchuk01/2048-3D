using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Domain.StaticData.Gameplay.Board
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "StaticData/Gameplay/BoardConfig")]
    public class BoardStaticData : ScriptableObject
    {
        public AssetReference BoardPrefab;
        public float BoardHalfWidth = 1.5f;
        [Min(1)] public int MaxCubesLimit = 15;
    }
}
