using System.Collections.Generic;
using System.Linq;
using Core.Services.ObjectPool;
using UnityEngine;

namespace Infrastructure.Services.ObjectPool
{
    public class ObjectPoolService<T> : IObjectPoolService<T>, IClearablePool where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _pool;

        public ObjectPoolService(T prefab, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _pool = new Queue<T>();
        }

        public T Get()
        {
            T item = _pool.Count > 0 ? _pool.Dequeue() : Object.Instantiate(_prefab, _parent);
            item.gameObject.SetActive(true);

            if (item.TryGetComponent<IPoolable>(out IPoolable poolable))
                poolable.OnSpawned();

            return item;
        }

        public void Return(T item)
        {
            if (item == null)
                return;

            if (item.TryGetComponent<IPoolable>(out IPoolable poolable))
                poolable.OnDespawned();

            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }

        public void Preload(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = Object.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                
                _pool.Enqueue(obj);
            }
        }
        
        public void Clear()
        {
            foreach (T item in _pool.Where(item => item != null)) 
                Object.Destroy(item.gameObject);

            _pool.Clear();
        }
    }
}