namespace Helpers
{
    using UnityEngine;
    using System;
    
    [Serializable]
    public struct SerializableVector3
    {
        public float X;
        public float Y;
        public float Z;
        
        public SerializableVector3(float rX, float rY, float rZ)
        {
            X = rX;
            Y = rY;
            Z = rZ;
        }

        public static implicit operator Vector3(SerializableVector3 rhs)
        {
            return new Vector3(rhs.X, rhs.Y, rhs.Z);
        }

        public static implicit operator SerializableVector3(Vector3 rhs)
        {
            return new SerializableVector3(rhs.x, rhs.y, rhs.z);
        }
    }
}