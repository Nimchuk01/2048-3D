using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Domain.StaticData.Gameplay.Board
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "StaticData/Gameplay/BoardConfig")]
    public class BoardStaticData : ScriptableObject
    {
        public AssetReference BoardPrefab;
    }
}
