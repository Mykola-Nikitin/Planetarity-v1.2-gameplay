using UnityEngine;

namespace Core.Sun
{
    [RequireComponent(typeof(Rigidbody))]
    public class SunComponent : MonoBehaviour
    {
        public float     GravityStrength = 10f;
        public Rigidbody Rigidbody;
        
        private SunHandler _handler;
        
        private void Awake()
        {
            _handler = new SunHandler(this);
        }

        private void OnDestroy()
        {
            _handler = null;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            _handler?.OnTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _handler?.OnTriggerExit(other);
        }

        private void FixedUpdate()
        {
            _handler?.Update();
        }
    }
}