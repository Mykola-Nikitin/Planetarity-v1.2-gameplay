using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.SignalBus
{
    public class MessageBus : IMessageBus
    {
        private static readonly Dictionary<Type, List<object>> _signals = new Dictionary<Type, List<object>>();

        public void DefineSignal<T>()
        {
            Type t = typeof(T);
            
            if (!_signals.ContainsKey(t))
            {
                _signals.Add(t, new List<object>());
            }
        }

        public void AddListener<T>(Action<T> callback)
        {
            Type t = typeof(T);

            if (!_signals.ContainsKey(t))
            {
                Debug.LogError($"Signal with type {t} was not defined.");
            }
            
            _signals[typeof(T)].Add(callback);
        }

        public void RemoveListener<T>(Action<T> callback)
        {
            if (_signals.TryGetValue(typeof(T), out List<object> callbacks))
            {
                callbacks.Remove(callback);
            }
        }

        public void Fire<T>(T signalData)
        {
            Type t = typeof(T);
            
            if (_signals.ContainsKey(t))
            {
                List<object> callbacks = _signals[t];

                for (var index = 0; index < callbacks.Count; )
                {
                    var callback = callbacks[index];
                    Action<T> action = callback as Action<T>;

                    if (action == null)
                    {
                        callbacks.RemoveAt(index);
                        
                        continue;
                    }

                    action(signalData);

                    index++;
                }
            }
        }
    }
}