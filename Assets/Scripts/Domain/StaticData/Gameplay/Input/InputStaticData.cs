using UnityEngine;

namespace Domain.StaticData.Gameplay.Input
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "StaticData/Gameplay/InputConfig")]
    public class InputStaticData : ScriptableObject
    {
        [Min(0f)] public float DragSensitivity = 1f;
        public Vector2 DragXLimits = new(-2.5f, 2.5f);
    }
}
