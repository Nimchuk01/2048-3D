using Core.Services.Progress;
using UnityEngine;
using Zenject;

namespace GameLogic.SaveSystem
{
    public class SaveProgress : MonoBehaviour
    {
        private IProgressStorage _progressStorage;

        [Inject]
        private void Construct(IProgressStorage progressStorage) => 
            _progressStorage = progressStorage;

        private void OnApplicationQuit() => Save();
        private void OnApplicationPause(bool pauseStatus) { if (pauseStatus) Save(); }
        private void OnApplicationFocus(bool hasFocus) { if (!hasFocus) Save(); }

        private void Save() => _progressStorage.Save();
    }
}