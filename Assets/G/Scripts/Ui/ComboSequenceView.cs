using System.Collections;
using System.Collections.Generic;
using G.Scripts.Services.ArrowSequence;
using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.Ui
{
    public class ComboSequenceView : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        [Header("Префабы стрелок")] [SerializeField]
        private ArrowIcon _upPrefab;

        [SerializeField] private ArrowIcon _downPrefab;
        [SerializeField] private ArrowIcon _leftPrefab;
        [SerializeField] private ArrowIcon _rightPrefab;

        [Header("Таймер")] [SerializeField] private GameObject _timerGameObject;
        [SerializeField] private Image _timerSlider; // ← Добавь сюда Slider
        [SerializeField] private Gradient _timerColor; // Опционально: цвет от зелёного к красному

        private readonly List<ArrowIcon> _currentIcons = new();

        private void Awake()
        {
            if (_timerSlider != null)
            {
                _timerSlider.fillAmount = 0f;
                _timerSlider.fillAmount = 1f;
                _timerSlider.fillAmount = 1f;
            }
        }

        public void ShowNewSequence(IReadOnlyList<ArrowDirection> sequence)
        {
            ClearIcons();
            _timerSlider.gameObject.SetActive(true);
            _timerGameObject.SetActive(true);
            foreach (var dir in sequence)
            {
                ArrowIcon prefab = GetPrefabForDirection(dir);
                if (prefab == null) continue;

                ArrowIcon icon = Instantiate(prefab, _container);
                icon.Init(dir);
                _currentIcons.Add(icon);
            }

            ResetTimerUI();
        }

        public void UpdateProgress(int correctCount)
        {
            for (int i = 0; i < _currentIcons.Count; i++)
            {
                if (i < correctCount)
                    _currentIcons[i].SetSuccess();
                else
                    _currentIcons[i].SetNormal();
            }
        }

        public void UpdateTimer(float normalizedTime) // 1 = полный таймер, 0 = время вышло
        {
            if (_timerSlider == null) return;

            _timerSlider.fillAmount = normalizedTime;

            if (_timerColor != null)
            {
                _timerSlider.color = _timerColor.Evaluate(normalizedTime);
            }
        }

        public void MarkAsCompleted()
        {
            foreach (var icon in _currentIcons)
                icon.SetSuccess();

            StartCoroutine(ClearAfterDelay(1f));
            HideTimer();
        }

        public void MarkFailedAt(int wrongIndex)
        {
            if (wrongIndex < 0 || wrongIndex >= _currentIcons.Count) return;

            for (int i = 0; i < wrongIndex; i++)
                _currentIcons[i].SetSuccess();

            _currentIcons[wrongIndex].SetFail();

            for (int i = wrongIndex + 1; i < _currentIcons.Count; i++)
                _currentIcons[i].SetNormal();

            StartCoroutine(ClearAfterDelay(1.5f));
            HideTimer();
        }

        private void ResetTimerUI()
        {
            if (_timerSlider != null)
                _timerSlider.fillAmount = 1f;
        }

        private void HideTimer()
        {
            if (_timerSlider != null)
                _timerSlider.fillAmount = 0f;
        }

        private IEnumerator ClearAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            ClearIcons();
        }

        private ArrowIcon GetPrefabForDirection(ArrowDirection dir)
        {
            return dir switch
            {
                ArrowDirection.Up => _upPrefab,
                ArrowDirection.Down => _downPrefab,
                ArrowDirection.Left => _leftPrefab,
                ArrowDirection.Right => _rightPrefab,
                _ => _upPrefab
            };
        }

        public void ClearIcons()
        {
            foreach (var icon in _currentIcons)
                Destroy(icon.gameObject);

            _currentIcons.Clear();
            _timerSlider.gameObject.SetActive(false);
            _timerGameObject.SetActive(false);
        }
    }
}