using System;
using G.Scripts.EnemyLogic;
using G.Scripts.Services.Spawner;
using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _damage;
        [SerializeField] private BulletSettings _settings;
        [SerializeField] private Rigidbody2D _rigidBody;
        
        private BulletMover _bulletMover;
        
        public event Action<IPoolable> e_onDespawnRequested;
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        public Rigidbody2D RigidBody => _rigidBody;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.CurrentColumn?.OnEnemyHit(enemy, _damage);

                Despawn();
            }
        }
        
        public void Despawn()
        {
            OnDespawned();
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