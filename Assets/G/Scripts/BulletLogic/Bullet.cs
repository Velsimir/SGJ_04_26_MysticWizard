using System;
using G.Scripts.EnemyLogic;
using G.Scripts.Services.SoundService;
using G.Scripts.Services.Spawner;
using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private Animator _animator;
        
        private BulletMover _bulletMover;
        private Sound _sound;
        
        public event Action<IPoolable> e_onDespawnRequested;
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        public float Speed => _speed;

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
            _animator?.SetBool("IsExplosion", true);
        }

        public void OnSpawned()
        {
            _sound = G.Instance.Services.GetService<Sound>();
            _bulletMover = new BulletMover(_rigidBody, _speed);
        }

        public void OnDespawned()
        {
            _sound.PlaySFX(_sound.пуф);
            _animator?.SetBool("IsExplosion", false);
            _bulletMover.Dispose();
            e_onDespawnRequested?.Invoke(this);
        }

        private void OnDestroy()
        {
            _bulletMover.Dispose();
        }
    }
}