using System.Collections.Generic;
using System.Linq;
using G.Scripts.Services.Spawner;
using UnityEngine;

namespace G.Scripts.EnemyLogic
{
    public class EnemyColumn : MonoBehaviour
    {
        private readonly List<Enemy> _enemies = new List<Enemy>();

        private EnemyColumnMover _mover;
        private float _moveSpeed = 0.5f;
        private bool _isActive = false;

        public IReadOnlyList<Enemy> Enemies => _enemies;

        public float MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                _moveSpeed = value;
                if (_mover != null)
                    _mover.MoveSpeed = value;
            }
        }

        public void Setup(List<Enemy> enemies, float moveSpeed)
        {
            _enemies.Clear();
            _enemies.AddRange(enemies);
            _moveSpeed = moveSpeed;

            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Initialize(this, i);
            }

            _isActive = true;

            _mover = new EnemyColumnMover(transform, this, _moveSpeed);
        }

        public void OnEnemyHit(Enemy hitEnemy, float damage)
        {
            hitEnemy?.TakeDamage(damage);
        }

        public void OnEnemyDied(Enemy enemy)
        {
            _enemies.Remove(enemy);

            if (_enemies.Count == 0)
                DestroyColumn();
        }

        public void DestroyColumn()
        {
            if (!_isActive) return;

            _isActive = false;

            _mover?.Dispose();
            _mover = null;

            foreach (var enemy in _enemies.ToList())
            {
                if (enemy != null)
                    G.Instance.Services.GetService<ISpawnerService>().Despawn(enemy);
            }

            _enemies.Clear();
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _mover?.Dispose();
        }
    }
}