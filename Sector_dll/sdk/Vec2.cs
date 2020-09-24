using Sector_dll.cheat;
using System;
using System.Reflection;

namespace Sector_dll.sdk
{
    class Vec2
    {

        public Vec2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        private static FieldInfo[] fields;

        public Vec2(object vec2)
        {
            if(fields == null)
                fields = vec2.GetType().GetFields();
            x = (double)fields[0].GetValue(vec2);
            y = (double)fields[1].GetValue(vec2);
        }

        public static Vec2 operator +(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vec2 operator -(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vec2 operator *(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vec2 operator *(Vec2 v1, double d)
        {
            return new Vec2(v1.x * d, v1.y * d);
        }

        public static Vec2 operator /(Vec2 v1, double d)
        {
            return new Vec2(v1.x / d, v1.y / d);
        }

        public double DistTo(Vec2 v)
        {
            return Math.Sqrt((x - v.x) * (x - v.x) + (y - v.y) * (y - v.y));
        }

        public double Len()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public double x;

        public double y;

        public override string ToString()
        {
            return $"Vec2({x}, {y})";
        }


    }
}
