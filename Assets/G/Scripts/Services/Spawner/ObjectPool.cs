using System;
using System.Collections.Generic;
using UnityEngine;

namespace G.Scripts.Services.Spawner
{
    public class ObjectPool<TPrefab> : IDisposable where TPrefab : class, IPoolable
    {
        private readonly Transform m_container;
        private readonly Queue<TPrefab> m_available = new Queue<TPrefab>();
        private readonly HashSet<TPrefab> m_active = new HashSet<TPrefab>();
        private readonly TPrefab m_prefab;

        private bool m_disposed;

        public ObjectPool(TPrefab prefab, Transform container = null)
        {
            m_prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
            m_container = container;
        }

        public TPrefab Prefab => m_prefab;
        public int ActiveCount => m_active.Count;
        public int InactiveCount => m_available.Count;

        public TPrefab Get(Transform customParent = null)
        {
            if (m_disposed)
            {
                Debug.LogError("ObjectPool is disposed. Cannot Get().");
                return null;
            }

            TPrefab instance = null;

            while (m_available.Count > 0)
            {
                instance = m_available.Dequeue();
                if (IsAlive(instance))
                    break;
                instance = null;
            }

            if (instance == null)
            {
                instance = InstantiateFromPrefab(customParent ?? m_container);
            }

            instance.GameObject.SetActive(true);

            instance.e_onDespawnRequested -= ReturnToPool;
            instance.e_onDespawnRequested += ReturnToPool;

            m_active.Add(instance);

            Transform targetParent = customParent ?? m_container;
            if (targetParent != null)
            {
                instance.Transform.SetParent(targetParent, worldPositionStays: false);
            }

            return instance;
        }

        public void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var instance = InstantiateFromPrefab(m_container);
                instance.GameObject.SetActive(false);
                m_available.Enqueue(instance);
            }
        }

        public void Clear()
        {
            // Уничтожаем активные
            foreach (var instance in m_active)
            {
                if (IsAlive(instance))
                {
                    instance.e_onDespawnRequested -= ReturnToPool;
                    UnityEngine.Object.Destroy(instance.GameObject);
                }
            }

            m_active.Clear();

            while (m_available.Count > 0)
            {
                TPrefab instance = m_available.Dequeue();
                if (IsAlive(instance))
                {
                    UnityEngine.Object.Destroy(instance.GameObject);
                }
            }
        }

        public void TrimExcess(int maxInactiveCount)
        {
            while (m_available.Count > maxInactiveCount)
            {
                TPrefab instance = m_available.Dequeue();
                if (IsAlive(instance))
                {
                    UnityEngine.Object.Destroy(instance.GameObject);
                }
            }
        }

        private void ReturnToPool(IPoolable poolable)
        {
            if (!(poolable is TPrefab instance))
                return;

            if (!m_active.Remove(instance))
            {
                Debug.LogWarning($"ObjectPool: instance {instance} is not active, ignoring return.");
                return;
            }

            instance.e_onDespawnRequested -= ReturnToPool;

            try
            {
                instance.OnDespawned();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in OnDespawned for {instance}: {e}");
            }

            instance.GameObject.SetActive(false);

            if (m_container != null)
            {
                instance.Transform.SetParent(m_container, worldPositionStays: false);
            }

            m_available.Enqueue(instance);
        }

        private TPrefab InstantiateFromPrefab(Transform parent)
        {
            GameObject go;

            if (parent != null)
                go = UnityEngine.Object.Instantiate(m_prefab.GameObject, parent);
            else
                go = UnityEngine.Object.Instantiate(m_prefab.GameObject);

            return go.GetComponent<TPrefab>();
        }

        private static bool IsAlive(TPrefab instance)
        {
            return instance != null && (instance as UnityEngine.Object) != null;
        }

        public void Dispose()
        {
            if (m_disposed) return;
            m_disposed = true;
            Clear();
        }
    }
}