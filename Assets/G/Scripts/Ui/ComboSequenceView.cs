using System.Collections;
using System.Collections.Generic;
using G.Scripts.Services.ArrowSequence;
using UnityEngine;

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

        private readonly List<ArrowIcon> _currentIcons = new();

        public void ShowNewSequence(IReadOnlyList<ArrowDirection> sequence)
        {
            ClearIcons();

            foreach (var dir in sequence)
            {
                ArrowIcon prefab = GetPrefabForDirection(dir);
                if (prefab == null) continue;

                ArrowIcon icon = Instantiate(prefab, _container);
                icon.Init(dir);
                _currentIcons.Add(icon);
            }
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

        public void MarkAsCompleted()
        {
            foreach (var icon in _currentIcons)
                icon.SetSuccess();

            // Исчезает через 1 секунду после успеха
            StartCoroutine(ClearAfterDelay(1f));
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
        }
    }
}