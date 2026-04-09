using System.IO;
using System.Threading;
using Core.Logging;
using Core.Services.Progress;
using Cysharp.Threading.Tasks;
using Domain.Progress;
using UnityEngine;

namespace Infrastructure.Services.Progress
{
    public class ProgressStorage : IProgressStorage
    {
        private const string FILE_NAME = "Progress.json";
        
        private IProgressService _progressService;

        public ProgressStorage(IProgressService progressService) => 
            _progressService = progressService;

        public UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            Load();
            
            DebugLogger.LogMessage(message: $"Loaded", sender: this);
            return UniTask.CompletedTask;
        }
        
        public void Save()
        {
            ProgressContainer container = 
                new ProgressContainer(_progressService.Settings, _progressService.Progress, _progressService.Economy);
            
            string json = JsonUtility.ToJson(container);
            File.WriteAllText(GetPath(), contents: json);
            
            DebugLogger.LogMessage($"Data saved to: {GetPath()}", sender: this);
        }

        public void Load()
        {
            string path = GetPath();
            
            if (!File.Exists(path))
            {
                DebugLogger.LogMessage($"No progress file found. Initializing with defaults.", sender: this);
                _progressService.SetData(new SettingsData(), new ProgressData(), new EconomyData());
                
                return;
            }

            string json = File.ReadAllText(path);
            ProgressContainer container = JsonUtility.FromJson<ProgressContainer>(json);

            _progressService.SetData(
                container.Settings ?? new SettingsData(), 
                container.Progress ?? new ProgressData(), 
                container.Economy ?? new EconomyData());
            
            DebugLogger.LogMessage($"Progress loaded from: {path}", sender: this);
        }

        public void Reset()
        {
            string path = GetPath();

            if (File.Exists(path))
            {
                File.Delete(path);
                DebugLogger.LogMessage($"Progress file deleted: {path}", sender: this);
            }
            else
            {
                DebugLogger.LogMessage($"No progress file found to delete at: {path}", sender: this);
            }
        }

        #region Private methods

        private static string GetPath() => Path.Combine(Application.persistentDataPath, FILE_NAME);

        #endregion

        #region Progress container

        [System.Serializable]
        private class ProgressContainer
        {
            public SettingsData Settings;
            public ProgressData Progress;
            public EconomyData Economy;

            public ProgressContainer(SettingsData s, ProgressData p, EconomyData e)
            {
                Settings = s;
                Progress = p;
                Economy = e;
            }
        }

        #endregion
    }
}