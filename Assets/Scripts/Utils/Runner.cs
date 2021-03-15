using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Runner : MonoBehaviour
    {
        private readonly Dictionary<int, Coroutine> _coroutines = new Dictionary<int, Coroutine>();

        public static Runner Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Run(Action callback, float delay)
        {
            Coroutine coroutine = StartCoroutine(Schedule(delay, callback));
            _coroutines[callback.GetHashCode()] = coroutine;
        }

        public void Run<T>(Action<T> callback, T item, float delay)
        {
            Coroutine coroutine = StartCoroutine(Schedule(delay, callback, item));
            _coroutines[callback.GetHashCode()] = coroutine;
        }

        public void Remove(Action callback)
        {
            int hash = callback.GetHashCode();
            Remove(hash);
        } 
        
        public void Remove<T>(Action<T> callback)
        {
            int hash = callback.GetHashCode();
            Remove(hash);
        }

        private void Remove(int hash)
        {
            if (_coroutines.TryGetValue(hash, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);

                _coroutines.Remove(hash);
            }
        }
        
        private IEnumerator Schedule(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);

            callback?.Invoke();
        }  

        private IEnumerator Schedule<T>(float delay, Action<T> callback, T item)
        {
            yield return new WaitForSeconds(delay);

            callback?.Invoke(item);
        }
    }
}