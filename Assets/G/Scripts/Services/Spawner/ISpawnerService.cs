using UnityEngine;

namespace G.Scripts.Services.Spawner
{
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