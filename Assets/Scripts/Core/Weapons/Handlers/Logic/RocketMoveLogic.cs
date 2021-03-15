using Core.Planets;
using UnityEngine;

namespace Core.Weapons
{
    public class RocketMoveLogic : IMoveLogic
    {
        public float Timer { get; }

        private readonly RocketComponent _component;
        private readonly float           _weight;
        private readonly float           _acceleration;
        
        public RocketMoveLogic(RocketComponent component, float weight, float acceleration)
        {
            _component    = component;
            _weight       = weight;
            _acceleration = acceleration;
        }
        
        public void Initialize(float timer)
        {
            
        }
        
        public void Move()
        {
            _component.Rigidbody.velocity += _component.transform.up * (_weight * _acceleration * Time.fixedDeltaTime);
        }
    }
}