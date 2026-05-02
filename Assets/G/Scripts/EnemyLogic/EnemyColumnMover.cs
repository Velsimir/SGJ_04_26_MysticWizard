using System;
using G.Scripts.Services.Update;
using UnityEngine;

namespace G.Scripts.EnemyLogic
{
    public class EnemyColumnMover : IUpdatable, IDisposable
    {
        private readonly Transform _transform;
        private readonly EnemyColumn _column;
        private float _moveSpeed;
        private bool _isDisposed;
    
        // Свойство для изменения скорости на лету
        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }
    
        public EnemyColumnMover(Transform transform, EnemyColumn column, float moveSpeed)
        {
            _transform = transform;
            _column = column;
            _moveSpeed = moveSpeed;
            _isDisposed = false;
        
            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
        }
    
        public void FixedUpdate(float deltaTime)
        { }
    
        public void Update(float deltaTime)
        {
            if (_isDisposed) return;
        
            _transform.Translate(Vector3.left * _moveSpeed * deltaTime);
        
            if (_transform.position.x < -15f)
            {
                _column.DestroyColumn();
            }
        }
    
        public void LateUpdate(float deltaTime)
        { }
    
        public void Dispose()
        {
            if (_isDisposed) return;
        
            _isDisposed = true;
            G.Instance.Services.GetService<IUpdateService>().Remove(this);
        }
    }
}