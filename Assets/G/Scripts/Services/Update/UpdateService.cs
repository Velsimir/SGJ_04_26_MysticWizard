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

        public float TimeScale
        {
            get => _timeScale;
            set
            {
                _timeScale = Mathf.Max(0f, value);
                Time.timeScale = _timeScale;
            }
        }

        private float CustomDeltaTime => Time.deltaTime * _timeScale;
        private float CustomFixedDeltaTime => Time.fixedDeltaTime * _timeScale;

        public void AddNew(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable) == false && _toAdd.Contains(updatable) == false)
                _toAdd.Add(updatable);
        }

        public void Remove(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable) && _toRemove.Contains(updatable) == false)
                _toRemove.Add(updatable);

            _toAdd.Remove(updatable);
        }

        private void FixedUpdate()
        {
            ApplyPendingChanges();

            foreach (var updatable in _updatables)
            {
                updatable.FixedUpdate(CustomFixedDeltaTime);
            }

            ApplyPendingChanges();
        }

        private void Update()
        {
            ApplyPendingChanges();

            foreach (var updatable in _updatables)
            {
                updatable.Update(CustomDeltaTime);
            }

            ApplyPendingChanges();
        }

        private void LateUpdate()
        {
            ApplyPendingChanges();

            foreach (var updatable in _updatables)
            {
                updatable.LateUpdate(CustomDeltaTime);
            }

            ApplyPendingChanges();
        }

        private void ApplyPendingChanges()
        {
            foreach (var toRemove in _toRemove)
            {
                _updatables.Remove(toRemove);
            }

            _toRemove.Clear();

            foreach (var toAdd in _toAdd)
            {
                _updatables.Add(toAdd);
            }

            _toAdd.Clear();
        }
    }
}