using Core.Services.ObjectPool;
using UnityEngine;

namespace Core.Factories
{
    public interface IObjectPoolFactory
    {
        IObjectPoolService<T> GetOrCreatePool<T>(T prefab, Transform parent = null) where T : Component;
        void Clear<T>() where T : Component;
        void ClearAll();
    }
}