using System;
using G.Scripts.Services.Input;
using G.Scripts.Services.Update;
using UnityEngine;

namespace G.Scripts.Services.ArrowSequence
{
    public class PlayerComboSystem : IUpdatable, IDisposable
    {
        private ArrowSequenceHandler _comboHandler;
        private IInputService _inputService;

        public void Start()
        {
            _inputService = G.Instance.Services.GetService<IInputService>();
            _comboHandler = new ArrowSequenceHandler(_inputService);

            // Подписываемся на события
            _comboHandler.OnSequenceCompleted += OnComboSuccess;
            _comboHandler.OnSequenceFailed += OnComboFailed;
            _comboHandler.OnProgressChanged += progress =>
                Debug.Log($"Прогресс комбо: {progress}/{_comboHandler.CurrentSequence.Count}");

            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
        }

        public void FixedUpdate(float deltaTime)
        { }

        public void Update(float deltaTime)
        {
            _comboHandler.Update(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        { }

        // Вызывай этот метод, когда хочешь дать игроку новую комбинацию
        public void GiveNewCombo(int length = 7)
        {
            _comboHandler.StartNewSequence(length);
        }

        private void OnComboSuccess()
        {
            Debug.Log("Комбинация выполнена успешно! → Сильная атака / Спецспособность");
            // Здесь твоя мощная логика (например, ультимативный выстрел)
        }

        private void OnComboFailed()
        {
            Debug.Log("Комбинация провалена");
            // Можно дать слабый эффект или ничего не делать
        }

        private void OnDestroy()
        {
            _comboHandler?.Dispose();
            G.Instance.Services.GetService<IUpdateService>().Remove(this);
        }

        public void Dispose()
        { }
    }
}