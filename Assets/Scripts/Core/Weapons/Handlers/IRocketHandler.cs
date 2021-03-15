using UnityEngine;

namespace Core.Weapons
{
    public interface IRocketHandler
    {
        RocketData RocketData { get; }
        int        Damage     { get; }
        int        OwnerId     { get; }
        
        void Initialize(int type, int ownerId, Vector3 ownerPosition, Vector3 lookPosition);
        void Load(RocketData rocketData);
        void Update();
        void OnTriggerEnter(Collider other);
        void Destroy();
    }
}