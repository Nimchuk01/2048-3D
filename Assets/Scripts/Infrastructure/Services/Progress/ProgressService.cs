using Core.Services.Progress;
using Domain.Progress;

namespace Infrastructure.Services.Progress
{
    public class ProgressService : IProgressService
    {
        public SettingsData Settings { get; private set; }
        public ProgressData Progress { get; private set; }
        public EconomyData Economy { get; private set; }
        
        public void SetData(SettingsData settings, ProgressData progress, EconomyData economy)
        {
            Settings = settings;
            Progress = progress;
            Economy = economy;
        }
    }
}