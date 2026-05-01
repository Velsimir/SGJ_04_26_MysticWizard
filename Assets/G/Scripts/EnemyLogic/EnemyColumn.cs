using System.Collections.Generic;
using System.Linq;
using G.Scripts.Services.Spawner;
using UnityEngine;

namespace G.Scripts.EnemyLogic
{
    public class EnemyColumn : MonoBehaviour
    {
        private readonly List<Enemy> _enemies = new List<Enemy>();

        private float _moveSpeed = 0.5f;
        private bool _isActive = false;

        public IReadOnlyList<Enemy> Enemies => _enemies;

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
        }

        private void Update()
        {
            if (!_isActive) return;

            transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);

            // Если колонна ушла за левый край
            if (transform.position.x < -15f)
            {
                DestroyColumn();
            }
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
            _isActive = false;

            foreach (var enemy in _enemies.ToList())
            {
                if (enemy != null)
                    G.Instance.Services.GetService<ISpawnerService>().Despawn(enemy);
            }

            _enemies.Clear();
            Destroy(gameObject);
        }
    }
}