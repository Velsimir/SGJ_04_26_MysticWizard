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

        // Текущая целевая комбинация
        private List<ArrowDirection> _currentSequence = new();
        private int _currentIndex = 0;

        // Настройки
        private float _inputTimeout = 4f; // сколько секунд даётся на следующую кнопку
        private float _timer = 0f;

        private bool _isActive = false;

        // События
        public event Action OnSequenceCompleted; // Игрок ввёл всё правильно
        public event Action OnSequenceFailed; // Ошибка в последовательности
        public event Action<int> OnProgressChanged; // Прогресс (сколько правильно ввели из N)

        public IReadOnlyList<ArrowDirection> CurrentSequence => _currentSequence;

        public ArrowSequenceHandler(IInputService inputService)
        {
            _inputService = inputService;

            // Подписываемся на Look (ты используешь Look для прицеливания/направления)
            _inputService.Move
                .Where(v => v.sqrMagnitude > 0.1f) // фильтруем "ничего не нажато"
                .Subscribe(OnDirectionInput);
        }

        // Активировать новую комбинацию
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
                return; // защита

            _timer = 0f; // сбрасываем таймер при каждом нажатии

            if (pressed == _currentSequence[_currentIndex])
            {
                // Правильная кнопка
                _currentIndex++;
                OnProgressChanged?.Invoke(_currentIndex);

                if (_currentIndex >= _currentSequence.Count)
                {
                    // Комбинация полностью введена правильно
                    OnSequenceCompleted?.Invoke();
                    _isActive = false;
                }
            }
            else
            {
                // Ошибка
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
                // Таймаут — считаем провалом
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

        // Вспомогательные методы
        public void SetTimeout(float newTimeout) => _inputTimeout = Mathf.Max(0.5f, newTimeout);
        public void Clear() => _isActive = false;
    }
}