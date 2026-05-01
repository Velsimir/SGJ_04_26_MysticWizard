using System;
using UnityEngine;

namespace G.Scripts.Services.Spawner
{
    public interface IPoolable
    {
        event Action<IPoolable> e_onDespawnRequested;
    
        GameObject GameObject { get; }
        Transform Transform { get; }

        void Despawn();
        void OnSpawned();
        void OnDespawned();
    }
}