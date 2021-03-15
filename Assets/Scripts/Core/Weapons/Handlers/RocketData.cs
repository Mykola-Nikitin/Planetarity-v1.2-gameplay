using System;
using Helpers;

namespace Core.Weapons
{
    [Serializable]
    public class RocketData
    {
        public int                 Type          { get; set; }
        public int                 OwnerId       { get; set; }
        public SerializableVector3 Target        { get; set; }
        public SerializableVector3 OwnerPosition { get; set; }
        public SerializableVector3 Position      { get; set; }
        public SerializableVector3 Velocity      { get; set; }
        public SerializableVector3 EulerRotation { get; set; }

        public RocketData Clone()
        {
            return new RocketData
            {
                Type          = Type,
                OwnerId       = OwnerId,
                Target        = Target,
                OwnerPosition = OwnerPosition,
                Position      = Position,
                Velocity      = Velocity,
                EulerRotation = EulerRotation
            };
        }
    }
}