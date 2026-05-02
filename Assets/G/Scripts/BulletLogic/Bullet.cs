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
        [SerializeField] private Animator _animator;
        
        private BulletMover _bulletMover;
        
        public event Action<IPoolable> e_onDespawnRequested;
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

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
            _bulletMover.Dispose();
            _animator.SetBool("IsExplosion", true);
        }

        public void OnSpawned()
        {
            _bulletMover = new BulletMover(_rigidBody, _settings);
        }

        public void OnDespawned()
        {
            _animator.SetBool("IsExplosion", false);
            e_onDespawnRequested?.Invoke(this);
        }
    }
}