using Core.Services.Initialization;

namespace Core.Services.Progress
{
    public interface IProgressStorage : IInitializableAsync
    {
        void Save();
        void Load();
        void Reset();
    }
}