using System;
using G.Scripts.Services.SoundService;
using G.Scripts.Services.Spawner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace G.Scripts.EnemyLogic
{
    public class Enemy : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _maxHp = 3f;
        [SerializeField] private Animator _animator;
        [SerializeField] private Sprite _skin;
        [SerializeField] private SpriteRenderer _visual;
        [SerializeField] private bool _isBoss;
        
        private float _currentHp;
        private EnemyColumn _currentColumn;
        private int _columnIndex;
        private Sound _sound;
        
        public event Action<IPoolable> e_onDespawnRequested;
        public static event Action Killed;

        public EnemyColumn CurrentColumn => _currentColumn;
        public int ColumnIndex => _columnIndex;
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        public bool IsBoss => _isBoss;

        public void Initialize(EnemyColumn column, int index, float hp = -1)
        {
            _sound = G.Instance.Services.GetService<Sound>();
            
            _currentColumn = column;
            _columnIndex = index;
            _currentHp = hp > 0 ? hp : _maxHp;
        }

        public void TakeDamage(float damage)
        {
            if (Random.Range(0, 2) == 0)
                _sound.PlaySFX(_sound.уронМыши1);
            else
                _sound.PlaySFX(_sound.уронМыши2);
            
            _currentHp -= damage;
                _animator.SetTrigger("Hit");
            if (_currentHp <= 0f)
                Die();
        }

        private void Die()
        {
            if (_isBoss == false)
            {
                if (_maxHp > 2)
                    _sound.PlaySFX(_sound.смертьМыши1);
                else
                    _sound.PlaySFX(_sound.смертьмыши2);
            }
            else
            {
                _sound.PlaySFX(_sound.смертьМышовура);
            }

            
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

        public void OnDestroy()
        {
            if (_isBoss)
                Killed?.Invoke();
        }
    }
}