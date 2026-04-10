using UnityEngine;

namespace GameLogic.Gameplay.Board
{
    public class BoardEntity : MonoBehaviour
    {
        [SerializeField] private Transform _cubesParent;

        public Transform CubesParent => _cubesParent;
    }
}
