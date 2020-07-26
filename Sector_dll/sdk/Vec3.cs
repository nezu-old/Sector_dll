﻿using Sector_dll.cheat;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sector_dll.sdk
{
    class Vec3
    {

        public static object New(double x, double y, double z)
        {
            return SignatureManager.Vec3_Constructor_double.Invoke(new object[] { x, y, z });
        }

        public Vec3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private static FieldInfo[] fields = null;

        public Vec3(object vec3)
        {
            if(fields == null)
            {
                FieldInfo[] all_fields = vec3.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                object t_vec = New(1, 2, 3);
                fields = new FieldInfo[3];
                foreach(FieldInfo fi in all_fields)
                {
                    double v = (double)fi.GetValue(t_vec);
                    fields[v == 1 ? 0 : v == 2 ? 1 : 2] = fi;
                }
            }
            x = (double)fields[0].GetValue(vec3);
            y = (double)fields[1].GetValue(vec3);
            z = (double)fields[2].GetValue(vec3);
        }

        public object ToInternal()
        {
            return New(x, y, z);
        }

        public double Len()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public override string ToString()
        {
            return string.Format("({0:F2},{1:F2},{2:F2})", x, y, z);
        }

        public static Vec3 operator +(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vec3 operator -(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vec3 operator *(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vec3 operator *(Vec3 v1, double d)
        {
            return new Vec3(v1.x * d, v1.y * d, v1.z * d);
        }

        public double x;

        public double y;

        public double z;

    }
}
