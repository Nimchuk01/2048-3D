using System;
using System.Collections.Generic;
using Core.Factories;
using Core.Services.ObjectPool;
using Infrastructure.Services.ObjectPool;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class ObjectPoolFactory : IObjectPoolFactory, IDisposable
    {
        private readonly Dictionary<Type, object> _pools = new();

        public IObjectPoolService<T> GetOrCreatePool<T>(T prefab, Transform parent = null) where T : Component
        {
            Type type = typeof(T);

            if (_pools.TryGetValue(type, out object pool))
                return (IObjectPoolService<T>)pool;

            ObjectPoolService<T> newPool = new ObjectPoolService<T>(prefab, parent);
            _pools[type] = newPool;
            
            return newPool;
        }

        public void Clear<T>() where T : Component
        {
            if (_pools.TryGetValue(typeof(T), out object pool) && pool is IClearablePool clearable)
            {
                clearable.Clear();
                _pools.Remove(typeof(T));
            }
        }

        public void ClearAll()
        {
            foreach (object pool in _pools.Values)
            {
                if (pool is IClearablePool clearable)
                    clearable.Clear();
            }

            _pools.Clear();
        }

        public void Dispose() => ClearAll();
    }
}