using System;
using G.Scripts.Services.Spawner;
using UnityEngine;

namespace G.Scripts.EnemyLogic
{
    public class Enemy : MonoBehaviour, IPoolable
    {
        public event Action<IPoolable> e_onDespawnRequested;

        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

        [SerializeField] private float _maxHp = 3f;

        private float _currentHp;
        private EnemyColumn _currentColumn; // Изменили название с Line на Column
        private int _columnIndex;

        public EnemyColumn CurrentColumn => _currentColumn;
        public int ColumnIndex => _columnIndex;

        public void Initialize(EnemyColumn column, int index, float hp = -1)
        {
            _currentColumn = column;
            _columnIndex = index;
            _currentHp = hp > 0 ? hp : _maxHp;
        }

        public void TakeDamage(float damage)
        {
            _currentHp -= damage;
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
            e_onDespawnRequested?.Invoke(this);
        }

        public void OnSpawned() => gameObject.SetActive(true);

        public void OnDespawned()
        {
            gameObject.SetActive(false);
            _currentColumn = null;
        }
    }
}