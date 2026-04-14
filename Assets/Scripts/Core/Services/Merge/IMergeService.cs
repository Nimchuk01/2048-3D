using Presentation.Gameplay.Cubes;

namespace Core.Services.Merge
{
    public interface IMergeService
    {
        bool CanMerge(CubeEntity source, CubeEntity target, float relativeVelocityMagnitude);
        void Merge(CubeEntity source, CubeEntity target);
    }
}
