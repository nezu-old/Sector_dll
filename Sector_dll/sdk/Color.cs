using System;
using System.Reflection;

namespace Sector_dll.sdk
{
    class Color : IEquatable<Color>
    {

        public static uint GetColor(object c)
        {
            return (uint)c.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)[0].GetValue(c);
        }

        public bool Equals(Color other)
        {
            if (other is null)
            {
                return false;
            }
            return ReferenceEquals(this, other) || other.color == color;
        }

        public Color(object o)
        {
            color = GetColor(o);
        }

        public Color(uint c)
        {
            color = c;
        }

        public Color(byte r, byte g, byte b, byte a = 255)
            : this((uint)(r << 0 | g << 8 | b << 16 | a << 24)) { }

        public Color(float r, float g, float b, float a = 1f) : 
            this((byte)(r * 255f), (byte)(g * 255f), (byte)(b * 255f), (byte)(a * 255f)) { }

        public static implicit operator uint(Color c) { return c.color; }

        public static implicit operator Color(uint c) { return new Color(c); }

        

        public uint color;

        public static readonly Color red = new Color(255, 0, 0);
        public static readonly Color green = new Color(0, 255, 0);
        public static readonly Color blue = new Color(0, 0, 255);
        public static readonly Color black = new Color(0, 0, 0);
        public static readonly Color white = new Color(255, 255, 255);

    }
}
