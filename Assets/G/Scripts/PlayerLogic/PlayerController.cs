using G.Scripts.Services.Input;
using G.Scripts.Services.Update;
using R3;
using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    public class PlayerController : IUpdatable
    {
        private readonly Transform _playerTransform;
        private readonly IInputService _inputService;
        private readonly PlayerSettings _settings;
        
        private Vector3 _moveDirection;
        
        public PlayerController(Transform playerTransform, PlayerSettings settings)
        {
            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
            _inputService = G.Instance.Services.GetService<IInputService>();
            
            _playerTransform = playerTransform;
            _settings = settings;
            
            _inputService.Look.Subscribe(SetMoveDirection);
        }

        private void SetMoveDirection(Vector2 a_vector2)
        {
            _moveDirection = a_vector2;
        }

        public void FixedUpdate(float deltaTime)
        { }

        public void Update(float deltaTime)
        {
            Move(deltaTime);
        }

        private void Move(float deltaTime)
        {
            Vector3 newPosition = _playerTransform.position + new Vector3(0, _moveDirection.y, 0) * _settings.speed * deltaTime;
            
            if (newPosition.y > _settings.topBorder)
                newPosition.y = _settings.topBorder;
            
            if (newPosition.y < _settings.botBorder)
                newPosition.y = _settings.botBorder;
            
            _playerTransform.position = newPosition;
        }

        public void LateUpdate(float deltaTime)
        { }
    }
}