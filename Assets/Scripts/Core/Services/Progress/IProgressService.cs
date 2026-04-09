using Domain.Progress;

namespace Core.Services.Progress
{
    public interface IProgressService
    {
        SettingsData Settings { get; }
        ProgressData Progress { get; }
        EconomyData Economy { get; }
        
        void SetData(SettingsData settings, ProgressData progress, EconomyData economy);
    }
}