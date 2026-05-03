using System.Collections.Generic;
using DG.Tweening;
using G.Scripts.EnemyLogic;
using G.Scripts.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.PlayerLogic
{
    public class PlayerDamageZone : MonoBehaviour
    {
        [Header("Настройки")] [SerializeField] private int _maxLeaksPer10Seconds = 5;
        [SerializeField] private float _timeWindow = 10f;

        [Header("Визуал")] [SerializeField] private Image _redScreen;
        [SerializeField] private float _redFlashDuration = 0.3f;
        [SerializeField] private float _redFlashAlpha = 0.6f;
        [SerializeField] private EndGameScreen _endGameScreen;
        
        private Queue<float> _leakTimestamps = new Queue<float>();
        private Player _player;

        private void Start()
        {
            _player = G.Instance.Player;

            if (_redScreen != null)
            {
                Color color = _redScreen.color;
                color.a = 0f;
                _redScreen.color = color;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
                RegisterLeak();
        }

        private void RegisterLeak()
        {
            float currentTime = Time.time;
            _leakTimestamps.Enqueue(currentTime);

            while (_leakTimestamps.Count > 0 &&
                   currentTime - _leakTimestamps.Peek() > _timeWindow)
            {
                _leakTimestamps.Dequeue();
            }

            if (_leakTimestamps.Count > _maxLeaksPer10Seconds)
            {
                ShowEndScreen();
            }

            if (_player != null)
            {
                // Можно добавить метод TakeDamage в Player, если его нет
                // Пока просто пример:
                // _player.TakeDamage(1);
            }

            FlashRedScreen();
        }

        private void ShowEndScreen()
        {
            _endGameScreen.Show();
        }

        private void FlashRedScreen()
        {
            if (_redScreen == null) return;

            _redScreen.DOKill();

            _redScreen.DOFade(_redFlashAlpha, _redFlashDuration / 2f)
                .SetUpdate(true)
                .OnComplete(() => { _redScreen.DOFade(0f, _redFlashDuration / 2f).SetUpdate(true); });
        }
    }
}