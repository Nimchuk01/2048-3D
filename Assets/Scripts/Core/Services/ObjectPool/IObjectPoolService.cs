using UnityEngine;

namespace Core.Services.ObjectPool
{
    public interface IObjectPoolService<T> where T : Component
    {
        T Get();
        void Return(T item);
        void Preload(int count);
    }
}