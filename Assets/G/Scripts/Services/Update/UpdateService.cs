using System.Collections.Generic;
using UnityEngine;

namespace G.Scripts.Services.Update
{
    public class UpdateService : MonoBehaviour, IUpdateService
    {
        private List<IUpdatable> _updatables = new List<IUpdatable>();
        private List<IUpdatable> _toAdd = new List<IUpdatable>();
        private List<IUpdatable> _toRemove = new List<IUpdatable>();

        public void AddNew(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable) == false && _toAdd.Contains(updatable) == false)
                _toAdd.Add(updatable);
        }

        public void Remove(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable) && _toRemove.Contains(updatable) == false)
                _toRemove.Add(updatable);
        }

        private void FixedUpdate()
        {
            ApplyPendingChanges();
        
            foreach (var updatable in _updatables)
            {
                updatable.FixedUpdate(Time.deltaTime);
            }
        
            ApplyPendingChanges();
        }

        private void Update()
        {
            ApplyPendingChanges();
        
            foreach (var updatable in _updatables)
            {
                updatable.Update(Time.deltaTime);
            }
        
            ApplyPendingChanges();
        }

        private void LateUpdate()
        {
            ApplyPendingChanges();
        
            foreach (var updatable in _updatables)
            {
                updatable.LateUpdate(Time.deltaTime);
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