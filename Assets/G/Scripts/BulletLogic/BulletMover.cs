using System;
using G.Scripts.Services.Update;
using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class BulletMover : IUpdatable, IDisposable
    {
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private BulletSettings _settings;
    
        public BulletMover(Transform transform, Rigidbody2D rigidbody, BulletSettings settings)
        {
            _transform = transform;
            _rigidbody = rigidbody;
            _settings = settings;
        
            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
        }

        public void FixedUpdate(float deltaTime)
        {
            _rigidbody.MovePosition(_rigidbody.position + Vector2.right * deltaTime * _settings.Speed);
        }

        public void Update(float deltaTime)
        { }

        public void LateUpdate(float deltaTime)
        { }

        public void Dispose()
        {
            G.Instance.Services.GetService<IUpdateService>().Remove(this);
        }
    }
}