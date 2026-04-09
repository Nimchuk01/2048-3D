using System.Collections;

namespace Core.Services.Coroutine
{
    public interface ICoroutineRunnerService
    {
        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);
    }
}