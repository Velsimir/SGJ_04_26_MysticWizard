using System;
using System.Collections.Generic;
using G.Scripts.Services.Input;
using R3;
using UnityEngine;

namespace G.Scripts.Services.ArrowSequence
{
    public class ArrowSequenceHandler : IDisposable
    {
        private readonly IInputService _inputService;

        private List<ArrowDirection> _currentSequence = new();
        private int _currentIndex = 0;

        private float _inputTimeout = 4f;
        private float _timer = 0f;

        private bool _isActive = false;

        public event Action OnSequenceCompleted;
        public event Action OnSequenceFailed;
        public event Action<int> OnProgressChanged;

        public IReadOnlyList<ArrowDirection> CurrentSequence => _currentSequence;
        public int CurrentIndex => _currentIndex;

        public ArrowSequenceHandler(IInputService inputService)
        {
            _inputService = inputService;

            _inputService.Move
                .Where(v => v.sqrMagnitude > 0.1f)
                .Subscribe(OnDirectionInput);
        }

        public void StartNewSequence(int length = 5)
        {
            if (length < 1) length = 3;

            _currentSequence.Clear();
            _currentIndex = 0;

            for (int i = 0; i < length; i++)
            {
                _currentSequence.Add(GetRandomDirection());
            }

            _timer = 0f;
            _isActive = true;

            OnProgressChanged?.Invoke(0);
            Debug.Log($"Новая последовательность ({length} кнопок): {string.Join(" → ", _currentSequence)}");
        }

        private void OnDirectionInput(Vector2 direction)
        {
            if (!_isActive) return;
            if (direction == Vector2.zero) return;
            
            ArrowDirection pressed = VectorToArrowDirection(direction);
            if (pressed == ArrowDirection.Up && _currentIndex >= _currentSequence.Count)
                return;

            _timer = 0f;

            if (pressed == _currentSequence[_currentIndex])
            {
                _currentIndex++;
                OnProgressChanged?.Invoke(_currentIndex);

                if (_currentIndex >= _currentSequence.Count)
                {
                    OnSequenceCompleted?.Invoke();
                    _isActive = false;
                }
            }
            else
            {
                OnSequenceFailed?.Invoke();
                _isActive = false;
            }
        }

        private ArrowDirection VectorToArrowDirection(Vector2 vec)
        {
            if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                return vec.x > 0 ? ArrowDirection.Right : ArrowDirection.Left;
            }
            else
            {
                return vec.y > 0 ? ArrowDirection.Up : ArrowDirection.Down;
            }
        }

        private ArrowDirection GetRandomDirection()
        {
            return (ArrowDirection)UnityEngine.Random.Range(0, 4);
        }

        public void Update(float deltaTime)
        {
            if (!_isActive) return;

            _timer += deltaTime;

            if (_timer >= _inputTimeout)
            {
                OnSequenceFailed?.Invoke();
                _isActive = false;
            }
        }

        public void Dispose()
        {
            _isActive = false;
            OnSequenceCompleted = null;
            OnSequenceFailed = null;
            OnProgressChanged = null;
        }

        public void SetTimeout(float newTimeout) => _inputTimeout = Mathf.Max(0.5f, newTimeout);
        public void Clear() => _isActive = false;
    }
}