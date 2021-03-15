using System;
using Helpers;

namespace Core.Planets
{
    [Serializable]
    public class PlanetData
    {
        public int  ID         { get; set; }
        public int  RocketType { get; set; }
        public bool IsPlayer   { get; set; }

        public float               OrbitDistance { get; set; }
        public float               Speed         { get; set; }
        public SerializableVector3 Position      { get; set; }
        public SerializableColor   Color         { get; set; }

        public float RechargeDuration { get; set; }
        public float CurrentRecharge  { get; set; }

        public int MaxHealth     { get; set; }
        public int CurrentHealth { get; set; }
        public float Timer { get; set; }

        public PlanetData Clone()
        {
            return new PlanetData
            {
                ID         = ID,
                RocketType = RocketType,
                IsPlayer   = IsPlayer,

                OrbitDistance = OrbitDistance,
                Speed         = Speed,
                Position      = Position,
                Color         = Color,

                RechargeDuration = RechargeDuration,
                CurrentRecharge  = CurrentRecharge,

                MaxHealth     = MaxHealth,
                CurrentHealth = CurrentHealth,
                Timer         = Timer
            };
        }
    }
}