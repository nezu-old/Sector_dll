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

        public double DistTo(Vec2 v)
        {
            return Math.Sqrt((x - v.x) * (x - v.x) + (y - v.y) * (y - v.y));
        }

        public double x;

        public double y;


    }
}
