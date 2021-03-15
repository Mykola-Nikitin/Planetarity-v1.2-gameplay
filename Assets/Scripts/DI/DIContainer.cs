using System;
using System.Collections.Generic;
using UnityEngine;

namespace DI
{
    public class DIContainer
    {
        private static Dictionary<Type, DIComponent> _typesComponents = new Dictionary<Type, DIComponent>();

        public static DIComponent For<T>() 
            where T : class
        {
            Type type = typeof(T);

            if (!_typesComponents.TryGetValue(type, out DIComponent diComponent))
            {
                diComponent = new DIComponent();
                _typesComponents.Add(type, diComponent);
            }

            return diComponent;
        }  
        
        public static DIComponent AsSingleton<T>() 
            where T : class
        {
            return For<T>().Self<T>();
        }

        public static T Get<T>(DIKey key = DIKey.None) 
            where T : class
        {
            Type type = typeof(T);

            if (!_typesComponents.TryGetValue(type, out DIComponent diComponent))
            {
                Debug.LogError($"Binding for {type} was not found");
                return default;
            }

           return diComponent.Get<T>(key);
        }  
        
        public static void Drop<TFrom, T>(DIKey key = DIKey.None)
            where T : class 
            where TFrom: class
        {
            Type type = typeof(TFrom);

            if (!_typesComponents.TryGetValue(type, out DIComponent diComponent))
            {
                Debug.LogWarning($"Binding for {type} was not found");
                return;
            }

            diComponent.Drop<T>(key);
        }
    }
}