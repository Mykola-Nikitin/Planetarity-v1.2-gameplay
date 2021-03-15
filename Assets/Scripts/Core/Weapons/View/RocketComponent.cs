using UnityEngine;

namespace Core.Weapons
{
    public class RocketComponent : MonoBehaviour
    {
        public Rigidbody    Rigidbody;
        public Collider     Collider;
        public MeshRenderer[] Parts;

        public int PLANET_LAYER { get; private set; }
        public int SUN_LAYER    { get; private set; }
        
        private IRocketHandler _rocketHandler;

        private void Awake()
        {
            PLANET_LAYER = LayerMask.NameToLayer("Planets");
            SUN_LAYER    = LayerMask.NameToLayer("Sun");
        }

        private void OnTriggerEnter(Collider other)
        {
            _rocketHandler?.OnTriggerEnter(other);
        }

        private void FixedUpdate()
        {
            _rocketHandler?.Update();
        }

        public void Setup(int type, int ownerId, Vector3 ownerPosition, Vector3 lookPosition)
        {
            _rocketHandler = new RocketHandler(this);
            _rocketHandler.Initialize(type, ownerId, ownerPosition, lookPosition);
        }   
        
        public void Setup(RocketData data)
        {
            _rocketHandler = new RocketHandler(this);
            _rocketHandler.Load(data);
        }

        public int GetDamage()
        {
            return _rocketHandler.Damage;
        }
        
        public int GetOwner()
        {
            return _rocketHandler.OwnerId;
        }
    }
}