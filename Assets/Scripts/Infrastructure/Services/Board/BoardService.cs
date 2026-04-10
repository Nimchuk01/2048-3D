using Core.Services.Board;
using UnityEngine;

namespace Infrastructure.Services.Board
{
    public class BoardService : IBoardService
    {
        public Vector3 CubeSpawnPosition { get; private set; }

        public void Initialize(Vector3 spawnPosition)
        {
            CubeSpawnPosition = spawnPosition;
        }
    }
}
