using System;
using UnityEngine;

namespace Helpers
{
    [Serializable]
    public struct SerializableColor
    {
        public float R;
        public float G;
        public float B;
        public float A;
        
        public SerializableColor(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static implicit operator Color(SerializableColor rhs)
        {
            return new Color(rhs.R, rhs.G, rhs.B, rhs.A);
        }

        public static implicit operator SerializableColor(Color rhs)
        {
            return new SerializableColor(rhs.r, rhs.g, rhs.b, rhs.a);
        }
    }
}