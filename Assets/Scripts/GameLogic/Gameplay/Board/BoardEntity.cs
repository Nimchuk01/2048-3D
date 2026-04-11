using UnityEngine;

namespace GameLogic.Gameplay.Board
{
    public class BoardEntity : MonoBehaviour
    {
        [SerializeField] private Transform _cubesParent;
        [SerializeField] private Transform[] _startCubeSpawnPoints;

        public Transform CubesParent => _cubesParent;
        public Transform[] StartCubeSpawnPoints => _startCubeSpawnPoints;
    }
}