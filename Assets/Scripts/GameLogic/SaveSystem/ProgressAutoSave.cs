using System.Collections;
using Core.Services.Progress;
using UnityEngine;
using Zenject;

namespace GameLogic.SaveSystem
{
    public class ProgressAutoSave : MonoBehaviour
    {
        private IProgressStorage _progressStorage;

        private Coroutine _autoSaveCoroutine;
        
        [Inject]
        private void Construct(IProgressStorage progressStorage) => 
            _progressStorage = progressStorage;
        
        private void Start() => 
            _autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
        
        private IEnumerator AutoSaveRoutine()
        {
            const float intervalSeconds = 30f;
            while (true)
            {
                yield return new WaitForSeconds(intervalSeconds);
                _progressStorage.Save();
            }
        }
        
    }
}