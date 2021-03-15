using System;
using System.Collections.Generic;
using UnityEngine;

namespace DI
{
    public class DIComponent
    {
        private readonly Dictionary<DIKey, object> _instances        = new Dictionary<DIKey, object>();
        private readonly Dictionary<DIKey, Type>   _typeDependencies = new Dictionary<DIKey, Type>();

        private bool _isSingleton;

        public DIComponent Use<T>(DIKey key = DIKey.None)
            where T : class
        {
            Type type = typeof(T);

            if (!_typeDependencies.TryGetValue(key, out Type existingType))
            {
                existingType = type;

                _typeDependencies.Add(key, existingType);
            }
            else
            {
                if (existingType != type)
                {
                    if (_instances.ContainsKey(key))
                    {
                        _instances.Remove(key);
                    }

                    _typeDependencies[key] = type;
                }
            }

            return this;
        }

        public DIComponent AsSingleton()
        {
            _isSingleton = true;

            return this;
        }

        public DIComponent Self<T>(DIKey key = DIKey.None)
            where T : class
        {
            Use<T>(key);
            AsSingleton();

            return this;
        }

        public T Get<T>(DIKey key = DIKey.None)
            where T : class
        {
            if (!_typeDependencies.ContainsKey(key))
            {
                Debug.LogError($"Couldn't find suitable type for key: {key}, type: {typeof(T)}");

                return default;
            }

            if (_isSingleton)
            {
                if (_instances.TryGetValue(key, out object value))
                {
                    return (T) value;
                }

                T singleInstance = Activator.CreateInstance<T>();

                _instances[key] = singleInstance;

                return singleInstance;
            }

            T instance = Activator.CreateInstance<T>();

            return instance;
        }

        public DIComponent Drop<T>(DIKey key = DIKey.None)
            where T : class
        {
            if (!_typeDependencies.TryGetValue(key, out Type existingType))
            {
                Debug.LogError($"Couldn't find suitable type for key: {key}, type: {typeof(T)}");

                return default;
            }

            if (_isSingleton)
            {
                if (_instances.TryGetValue(key, out object _))
                {
                    _instances.Remove(key);
                }
            }

            _typeDependencies.Remove(key);

            return this;
        }

        public DIComponent Resolve(DIKey key = DIKey.None)
        {
            if (_isSingleton)
            {
                if (!_typeDependencies.TryGetValue(key, out Type existingType))
                {
                    Debug.LogError($"Couldn't resolve type for key: {key}");

                    return this;
                }

                object singleInstance = Activator.CreateInstance(existingType);

                if (!_instances.TryGetValue(key, out object _))
                {
                    _instances[key] = singleInstance;
                }
                else
                {
                    Debug.Log($"Dependency for key: {key} was already resolved");
                }
            }

            return this;
        }
    }
}