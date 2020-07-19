using Sector_dll.cheat;
using System;
using System.Reflection;

namespace Sector_dll.sdk
{
    class Vec2
    {

        public static object New(double x, double y)
        {
            return SignatureManager.Vec2_Constructor.Invoke(new object[] { x, y });
        }

        public Vec2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2(object vec2)
        {
            Type t = vec2.GetType();
            if (t == SignatureManager.Vec2)
            {
                FieldInfo[] fields = t.GetFields();
                x = (double)fields[0].GetValue(vec2);
                y = (double)fields[1].GetValue(vec2);
            }
            else throw new NotSupportedException("can not cast from " + t.ToString() + " to vec2");
        }

        public object ToInternal()
        {
            return New(x, y);
        }

        public double x;

        public double y;


    }
}
