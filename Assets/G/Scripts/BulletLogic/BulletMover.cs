using System;
using G.Scripts.Services.Update;
using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class BulletMover : IUpdatable, IDisposable
    {
        private Rigidbody2D _rigidbody;
        private float _speed;
        
        public BulletMover(Rigidbody2D rigidbody, float speed)
        {
            _rigidbody = rigidbody;
            _speed = speed;
        
            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
        }

        public void FixedUpdate(float deltaTime)
        {
            _rigidbody.MovePosition(_rigidbody.position + Vector2.right * deltaTime * _speed);
        }

        public void Update(float deltaTime)
        { }

        public void LateUpdate(float deltaTime)
        { }

        public void Dispose()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            G.Instance.Services.GetService<IUpdateService>().Remove(this);
        }
    }
}