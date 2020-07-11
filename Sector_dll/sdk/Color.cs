using Sector_dll.cheat;
using System.Net.Http;
using System.Reflection;

namespace Sector_dll.sdk
{
    class Color
    {

        public static object New(uint color){
            if (SignatureManager.Color_Constructor_uint == null) 
                return null;
            return SignatureManager.Color_Constructor_uint.Invoke(new object[] { color });
        }

        public static object New(byte r, byte g, byte b, byte a = 255)
        {
            if (SignatureManager.Color_Constructor_byte == null)
                return null;
            return SignatureManager.Color_Constructor_byte.Invoke(new object[] { r, g, b, a });
        }

        public static object New(float r, float g, float b, float a = 1f)
        {
            if (SignatureManager.Color_Constructor_uint == null)
                return null;
            return SignatureManager.Color_Constructor_uint.Invoke(new object[] { r, g, b, a });
        }

        public static uint GetColor(object c)
        {
            return (uint)c.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)[0].GetValue(c);
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

        public uint color;

        public static readonly Color red = new Color(255, 0, 0);
        public static readonly Color green = new Color(0, 255, 0);
        public static readonly Color blue = new Color(0, 0, 255);
        public static readonly Color black = new Color(0, 0, 0);
        public static readonly Color white = new Color(255, 255, 255);

    }
}
