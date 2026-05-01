using System;
using G.Scripts.Services.Spawner;
using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private BulletSettings _settings;
        [SerializeField] private Rigidbody2D _rigidBody;
        
        private BulletMover _bulletMover;
        
        public event Action<IPoolable> e_onDespawnRequested;
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        public Rigidbody2D RigidBody => _rigidBody;

        public void Despawn()
        {
            Debug.Log("Bullet Despawned");
            OnDespawned();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Bullet triggered with: {other.gameObject.name}");
        }
    
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"Bullet collided with: {collision.gameObject.name}");
        }

        public void OnSpawned()
        {
            _bulletMover = new BulletMover(transform, _rigidBody, _settings);
        }

        public void OnDespawned()
        {
            _bulletMover.Dispose();
            e_onDespawnRequested?.Invoke(this);
        }
    }
}