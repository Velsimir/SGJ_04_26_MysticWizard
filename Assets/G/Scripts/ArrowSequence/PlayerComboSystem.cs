using System;
using G.Scripts.Services.Input;
using G.Scripts.Services.Update;
using G.Scripts.Ui;
using UnityEngine;

namespace G.Scripts.Services.ArrowSequence
{
    public class PlayerComboSystem : IUpdatable, IDisposable, IService
    {
        private readonly ArrowSequenceHandler _comboHandler;
        private readonly ComboSequenceView _comboView;
        private readonly IInputService _inputService;

        private bool _isWorking;
        
        public event Action OnSuccess;
        public event Action OnFail;

        public PlayerComboSystem(ComboSequenceView comboView)
        {
            _comboView = comboView;
            _inputService = G.Instance.Services.GetService<IInputService>();
            _comboHandler = new ArrowSequenceHandler(_inputService);

            _comboHandler.OnSequenceCompleted += OnComboSuccess;
            _comboHandler.OnSequenceFailed += OnComboFailed;
            _comboHandler.OnProgressChanged += OnProgressChanged;
        }

        public void FixedUpdate(float deltaTime)
        { }

        public void Update(float deltaTime)
        {
            if (_isWorking == false)
                return;
            
            _comboHandler.Update(deltaTime);

            // Обновляем слайдер таймера
            if (_comboView != null && _comboHandler.IsActive)
            {
                float normalizedTime = 1f - (_comboHandler.Timer / _comboHandler.InputTimeout);
                _comboView.UpdateTimer(Mathf.Clamp01(normalizedTime));
            }
        }

        public void LateUpdate(float deltaTime)
        {
        }

        public void GiveNewCombo(int length = 7)
        {
            _comboHandler.StartNewSequence(length);
            _comboView?.ShowNewSequence(_comboHandler.CurrentSequence);
        }
        
        public void StartSystem()
        {
            _isWorking = true;
        }
        
        public void StopSystem()
        {
            _isWorking = false;
        }

        private void OnProgressChanged(int correctCount)
        {
            _comboView?.UpdateProgress(correctCount);
        }

        private void OnComboSuccess()
        {
            Debug.Log("Комбинация выполнена успешно!");
            _comboView?.MarkAsCompleted();
            OnSuccess?.Invoke();
        }

        private void OnComboFailed()
        {
            Debug.Log("Комбинация провалена");

            int failIndex = _comboHandler.CurrentIndex;
            _comboView?.MarkFailedAt(failIndex);

            OnFail?.Invoke();
        }

        public void Dispose()
        {
            _comboHandler?.Dispose();
            G.Instance.Services.GetService<IUpdateService>().Remove(this);

            _comboHandler.OnSequenceCompleted -= OnComboSuccess;
            _comboHandler.OnSequenceFailed -= OnComboFailed;
            _comboHandler.OnProgressChanged -= OnProgressChanged;
        }
    }
}