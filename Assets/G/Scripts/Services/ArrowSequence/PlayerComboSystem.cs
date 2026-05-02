using System;
using G.Scripts.PlayerLogic;
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

        public void Update(float deltaTime) => _comboHandler.Update(deltaTime);
        
        public void LateUpdate(float deltaTime)
        { }

        public void GiveNewCombo(int length = 7)
        {
            _comboHandler.StartNewSequence(length);
            _comboView?.ShowNewSequence(_comboHandler.CurrentSequence);
        }

        private void OnProgressChanged(int correctCount)
        {
            _comboView?.UpdateProgress(correctCount);
        }

        private void OnComboSuccess()
        {
            Debug.Log("Комбинация выполнена успешно!");
            _comboView?.MarkAsCompleted();
            _comboView?.MarkAsCompleted();
            OnSuccess?.Invoke();
        }

        private void OnComboFailed()
        {
            Debug.Log("Комбинация провалена");
    
            int failIndex = _comboHandler.CurrentIndex;
    
            _comboView?.MarkFailedAt(failIndex);
    
            _comboView?.MarkFailedAt(failIndex);
            OnFail?.Invoke();
        }

        public void Dispose()
        {
            _comboHandler?.Dispose();

            G.Instance.Services.GetService<IUpdateService>().Remove(this);

            // Отписываемся
            _comboHandler.OnSequenceCompleted -= OnComboSuccess;
            _comboHandler.OnSequenceFailed -= OnComboFailed;
            _comboHandler.OnProgressChanged -= OnProgressChanged;
        }
    }
}