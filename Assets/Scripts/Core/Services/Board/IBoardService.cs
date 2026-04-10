using UnityEngine;

namespace Core.Services.Board
{
    public interface IBoardService
    {
        Vector3 CubeSpawnPosition { get; }
        
        void Initialize(Vector3 spawnPosition);
    }
}
