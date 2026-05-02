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
        private readonly Animator _animator;

        private Vector2 _moveDirection;
        private bool _isWalking;

        public PlayerController(Rigidbody2D rigidbody, Animator animator, PlayerSettings settings)
        {
            _rigidbody = rigidbody;
            _animator = animator;
            _settings = settings;

            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
            _inputService = G.Instance.Services.GetService<IInputService>();

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
            // Обновляем анимацию каждый кадр
            UpdateWalkingAnimation();
        }

        private void UpdateWalkingAnimation()
        {
            bool shouldWalk = _moveDirection.sqrMagnitude > 0.01f; // небольшая мёртвая зона

            if (shouldWalk != _isWalking)
            {
                _isWalking = shouldWalk;
                _animator.SetBool("IsWalking", _isWalking);
            }
        }

        private void Move(float deltaTime)
        {
            Vector2 targetPosition = _rigidbody.position +
                                     new Vector2(0, _moveDirection.y) * _settings.Speed * deltaTime;

            targetPosition.y = Mathf.Clamp(targetPosition.y, _settings.BotBorder, _settings.TopBorder);

            _rigidbody.MovePosition(targetPosition);
        }

        public void LateUpdate(float deltaTime)
        {
        }

        public void Dispose()
        {
            G.Instance.Services.GetService<IUpdateService>().Remove(this);

            // Сбрасываем анимацию при уничтожении
            if (_animator != null)
                _animator.SetBool("IsWalking", false);
        }
    }
}