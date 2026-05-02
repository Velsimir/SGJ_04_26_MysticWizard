using G.Scripts.Services.Input;
using G.Scripts.Services.Update;
using R3;
using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    public class PlayerController : IUpdatable
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly IInputService _inputService;
        private readonly PlayerSettings _settings;

        private Vector2 _moveDirection;

        public PlayerController(Rigidbody2D rigidbody, PlayerSettings settings)
        {
            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
            _inputService = G.Instance.Services.GetService<IInputService>();

            _rigidbody = rigidbody;
            _settings = settings;

            _rigidbody.gravityScale = 0;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

            _inputService.Look.Subscribe(SetMoveDirection);
        }

        private void SetMoveDirection(Vector2 direction)
        {
            _moveDirection = direction;
        }

        public void FixedUpdate(float deltaTime)
        {
            Move(deltaTime);
        }

        public void Update(float deltaTime)
        {
        }

        private void Move(float deltaTime)
        {
            Vector2 targetPosition =
                _rigidbody.position + new Vector2(0, _moveDirection.y) * _settings.Speed * deltaTime;

            if (targetPosition.y > _settings.TopBorder)
                targetPosition.y = _settings.TopBorder;

            if (targetPosition.y < _settings.BotBorder)
                targetPosition.y = _settings.BotBorder;

            _rigidbody.MovePosition(targetPosition);
        }

        public void LateUpdate(float deltaTime)
        {
        }

        public void Dispose()
        {
            G.Instance.Services.GetService<IUpdateService>().Remove(this);
        }
    }
}