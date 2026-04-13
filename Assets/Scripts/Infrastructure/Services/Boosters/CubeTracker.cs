using System.Collections.Generic;
using Core.Services.Boosters;
using GameLogic.Gameplay.Cubes;

namespace Infrastructure.Services.Boosters
{
    public class CubeTracker : ICubeTracker
    {
        private readonly List<CubeEntity> _cubes = new();
        
        public IReadOnlyList<CubeEntity> Cubes => _cubes;

        public void Register(CubeEntity cube)
        {
            if (!_cubes.Contains(cube))
                _cubes.Add(cube);
        }

        public void Unregister(CubeEntity cube)
        {
            _cubes.Remove(cube);
        }

        public void Clear()
        {
            _cubes.Clear();
        }
    }
}
