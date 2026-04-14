using System.Collections.Generic;
using Presentation.Gameplay.Cubes;

namespace Core.Services.Cube
{
    public interface ICubeTracker
    {
        IReadOnlyList<CubeEntity> Cubes { get; }
        void Register(CubeEntity cube);
        void Unregister(CubeEntity cube);
        void Clear();
    }
}
