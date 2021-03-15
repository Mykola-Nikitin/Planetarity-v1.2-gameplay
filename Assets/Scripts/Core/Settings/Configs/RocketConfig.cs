using UnityEngine;

namespace Core.Settings
{
    [CreateAssetMenu(fileName = "Rocket.asset", menuName = "ScriptableObjects/Create rocket config", order = 0)]
    public class RocketConfig : ScriptableObject
    {
        public int     Type;
        public float   Acceleration;
        public float   Weight;
        public float   RechargeTime;
        public int     Damage;
        public float   Lifetime;
        public Color   Color;
    }
}