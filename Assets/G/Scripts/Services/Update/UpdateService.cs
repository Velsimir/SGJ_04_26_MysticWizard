using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G.Scripts.Services.Update
{
    public class UpdateService : MonoBehaviour, IUpdateService
    {
        [SerializeField] private float _timeScale = 1f;

        private List<IUpdatable> _updatables = new List<IUpdatable>();
        private List<IUpdatable> _toAdd = new List<IUpdatable>();
        private List<IUpdatable> _toRemove = new List<IUpdatable>();

        private Coroutine _lerpCoroutine;

        public float TimeScale
        {
            get => _timeScale;
            private set
            {
                _timeScale = Mathf.Max(0f, value);
            }
        }

        private float CustomDeltaTime => Time.deltaTime * _timeScale;
        private float CustomFixedDeltaTime => Time.fixedDeltaTime * _timeScale;

        #region TimeScale Control

        public void SetTimeScale(float targetScale)
        {
            StopLerp();
            TimeScale = targetScale;
        }

        public void LerpTimeScale(float targetScale, float duration)
        {
            StopLerp();
            _lerpCoroutine = StartCoroutine(LerpTimeScaleRoutine(targetScale, duration));
        }

        private IEnumerator LerpTimeScaleRoutine(float targetScale, float duration)
        {
            float startScale = _timeScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime; // важно использовать unscaledDeltaTime!
                float t = Mathf.Clamp01(elapsed / duration);
                TimeScale = Mathf.Lerp(startScale, targetScale, t);
                yield return null;
            }

            TimeScale = targetScale; // точная установка в конце
        }

        private void StopLerp()
        {
            if (_lerpCoroutine != null)
            {
                StopCoroutine(_lerpCoroutine);
                _lerpCoroutine = null;
            }
        }

        #endregion

        #region Updatable Management

        public void AddNew(IUpdatable updatable)
        {
            if (!_updatables.Contains(updatable) && !_toAdd.Contains(updatable))
                _toAdd.Add(updatable);
        }

        public void Remove(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable) && !_toRemove.Contains(updatable))
                _toRemove.Add(updatable);

            _toAdd.Remove(updatable);
        }

        #endregion

        private void FixedUpdate()
        {
            ApplyPendingChanges();

            foreach (var updatable in _updatables)
                updatable.FixedUpdate(CustomFixedDeltaTime);

            ApplyPendingChanges();
        }

        private void Update()
        {
            ApplyPendingChanges();

            foreach (var updatable in _updatables)
                updatable.Update(CustomDeltaTime);

            ApplyPendingChanges();
        }

        private void LateUpdate()
        {
            ApplyPendingChanges();

            foreach (var updatable in _updatables)
                updatable.LateUpdate(CustomDeltaTime);

            ApplyPendingChanges();
        }

        private void ApplyPendingChanges()
        {
            foreach (var item in _toRemove)
                _updatables.Remove(item);
            _toRemove.Clear();

            foreach (var item in _toAdd)
                _updatables.Add(item);
            _toAdd.Clear();
        }

        private void OnDestroy()
        {
            StopLerp();
        }
    }
}