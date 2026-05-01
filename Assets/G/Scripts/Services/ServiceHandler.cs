using System;
using System.Collections.Generic;
using UnityEngine;

namespace G.Scripts.Services
{
    public class ServiceHandler
    {
        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public bool TryGetService<T>(out T serviceInstance) where T : IService
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                serviceInstance = (T)service;
                return true;
            }
        
            serviceInstance = default;
            return false;
        }

        public T GetService<T>() where T : IService
        {
            if (TryGetService<T>(out var service))
                return service;
        
            Debug.LogError($"Service of type {typeof(T)} not found!");
            return default;
        }

        public void AddService<T>(T service) where T : IService
        {
            _services[typeof(T)] = service;
        }

        public void RemoveService<T>() where T : IService
        {
            _services.Remove(typeof(T));
        }
    }
}