using System.Collections.Generic;
using GameLogic.Gameplay.Cubes;

namespace Core.Services.Boosters
{
    public interface ICubeTracker
    {
        IReadOnlyList<CubeEntity> Cubes { get; }
        void Register(CubeEntity cube);
        void Unregister(CubeEntity cube);
        void Clear();
    }
}
