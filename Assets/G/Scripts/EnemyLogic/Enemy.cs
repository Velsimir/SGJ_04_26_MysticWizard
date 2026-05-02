using System;
using G.Scripts.Services.Spawner;
using UnityEngine;

namespace G.Scripts.EnemyLogic
{
    public class Enemy : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _maxHp = 3f;
        [SerializeField] private Animator _animator;
        [SerializeField] private Sprite _skin;
        [SerializeField] private SpriteRenderer _visual;
        
        private float _currentHp;
        private EnemyColumn _currentColumn;
        private int _columnIndex;
        
        public event Action<IPoolable> e_onDespawnRequested;

        public EnemyColumn CurrentColumn => _currentColumn;
        public int ColumnIndex => _columnIndex;
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

        public void Initialize(EnemyColumn column, int index, float hp = -1)
        {
            _currentColumn = column;
            _columnIndex = index;
            _currentHp = hp > 0 ? hp : _maxHp;
        }

        public void TakeDamage(float damage)
        {
            _currentHp -= damage;
                _animator.SetTrigger("Hit");
            if (_currentHp <= 0f)
                Die();
        }

        private void Die()
        {
            _currentColumn?.OnEnemyDied(this);
            Despawn();
        }

        public void Despawn()
        {
            _animator.SetBool("IsDead", true);
        }

        public void OnSpawned()
        {
            gameObject.SetActive(true);   
        }

        public void OnDespawned()
        {
            e_onDespawnRequested?.Invoke(this);
            _animator.SetBool("IsDead", false);
            _visual.sprite = _skin;
            gameObject.SetActive(false);
            _currentColumn = null;
        }
    }
}