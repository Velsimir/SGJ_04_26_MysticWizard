using System;
using System.Collections.Generic;
using UnityEngine;

namespace G.Scripts.Services.Spawner
{
    public class SpawnerService : IDisposable, ISpawnerService
    {
        private readonly Transform m_defaultContainer;

        // object — потому что внутри лежат ObjectPool<T> для разных T
        private readonly Dictionary<object, object> m_pools = new Dictionary<object, object>();

        private bool m_disposed;

        public SpawnerService(Transform defaultContainer = null)
        {
            m_defaultContainer = defaultContainer;
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null)
            where T : class, IPoolable
        {
            if (m_disposed)
            {
                Debug.LogError("SpawnerService is disposed.");
                return null;
            }

            var pool = GetOrCreatePool(prefab);

            Transform actualParent = parent ?? m_defaultContainer;
            var instance = pool.Get(actualParent);

            instance.Transform.SetPositionAndRotation(position, rotation);

            if (parent != null && actualParent != parent)
            {
                instance.Transform.SetParent(parent, worldPositionStays: true);
            }

            instance.OnSpawned();

            return instance;
        }

        public T Spawn<T>(T prefab, Transform spawnPoint, Transform parent = null)
            where T : class, IPoolable
        {
            return Spawn(prefab, spawnPoint.position, spawnPoint.rotation);
        }

        private ObjectPool<T> GetOrCreatePool<T>(T prefab) where T : class, IPoolable
        {
            if (!m_pools.TryGetValue(prefab, out var poolObj))
            {
                var newPool = new ObjectPool<T>(prefab, m_defaultContainer);
                m_pools[prefab] = newPool;
                return newPool;
            }

            return (ObjectPool<T>)poolObj;
        }

        public void Despawn<T>(T obj) where T : class, IPoolable
        {
            obj.Despawn();
        }

        public void Prewarm<T>(T prefab, int count) where T : class, IPoolable
        {
            var pool = GetOrCreatePool(prefab);
            pool.Prewarm(count);
        }

        public void Clear<T>(T prefab) where T : class, IPoolable
        {
            if (m_pools.TryGetValue(prefab, out var poolObj))
            {
                var pool = (ObjectPool<T>)poolObj;
                pool.Clear();
                m_pools.Remove(prefab);
            }
        }

        public void TrimExcess<T>(T prefab, int maxCount) where T : class, IPoolable
        {
            if (m_pools.TryGetValue(prefab, out var poolObj))
            {
                var pool = (ObjectPool<T>)poolObj;
                pool.TrimExcess(maxCount);
            }
        }

        public int GetActiveCount<T>(T prefab) where T : class, IPoolable
        {
            if (m_pools.TryGetValue(prefab, out var poolObj))
            {
                return ((ObjectPool<T>)poolObj).ActiveCount;
            }

            return 0;
        }

        public int GetInactiveCount<T>(T prefab) where T : class, IPoolable
        {
            if (m_pools.TryGetValue(prefab, out var poolObj))
            {
                return ((ObjectPool<T>)poolObj).InactiveCount;
            }

            return 0;
        }

        public void ClearAll()
        {
            foreach (var pool in m_pools.Values)
            {
                if (pool is IDisposable disposable)
                    disposable.Dispose();
            }

            m_pools.Clear();
        }

        public void Dispose()
        {
            if (m_disposed) return;
            m_disposed = true;
            ClearAll();
        }
    }

    public interface ISpawnerService : IService
    {
        T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null)
            where T : class, IPoolable;
        
        T Spawn<T>(T prefab, Transform spawnPoint, Transform parent = null)
            where T : class, IPoolable;

        void Despawn<T>(T obj) where T : class, IPoolable;

        void Prewarm<T>(T prefab, int count) where T : class, IPoolable;

        void Clear<T>(T prefab) where T : class, IPoolable;

        void TrimExcess<T>(T prefab, int maxCount) where T : class, IPoolable;

        int GetActiveCount<T>(T prefab) where T : class, IPoolable;

        int GetInactiveCount<T>(T prefab) where T : class, IPoolable;
        
        void ClearAll();
    }
}