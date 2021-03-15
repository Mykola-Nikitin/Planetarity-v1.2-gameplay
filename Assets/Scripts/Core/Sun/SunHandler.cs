using System.Collections.Generic;
using UnityEngine;

namespace Core.Sun
{
    public class SunHandler
    {
        private readonly SunComponent       _sunComponent;
        private readonly HashSet<Rigidbody> _rigidbodies = new HashSet<Rigidbody>();
        private readonly List<Rigidbody>    _used        = new List<Rigidbody>();

        private bool _activated;

        public SunHandler(SunComponent sunComponent)
        {
            _sunComponent = sunComponent;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody)
            {
                _rigidbodies.Add(other.attachedRigidbody);
            }
        }   
        
        public void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody)
            {
                _rigidbodies.Remove(other.attachedRigidbody);
            }
        }

        public void Update()
        {
            foreach (Rigidbody affectedRigidbody in _rigidbodies)
            {
                if (!affectedRigidbody.gameObject.activeInHierarchy)
                {
                    _used.Add(affectedRigidbody);
                    continue;
                }
                
                Vector3 forceDirection = (_sunComponent.transform.position - affectedRigidbody.position).normalized;
                
                float distanceSqr = (_sunComponent.transform.position - affectedRigidbody.position).magnitude;
                float strength = _sunComponent.GravityStrength * _sunComponent.Rigidbody.mass * affectedRigidbody.mass / distanceSqr;

                affectedRigidbody.AddForce(forceDirection * strength);
            }

            if (_used.Count > 0)
            {
                for (var i = _used.Count - 1; i >= 0; i--)
                {
                    _rigidbodies.Remove(_used[i]);
                    _used.RemoveAt(i);
                }
            }
        }
    }
}