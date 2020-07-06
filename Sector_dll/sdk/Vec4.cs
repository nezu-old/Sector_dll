using Sector_dll.cheat;
using System;
using System.Reflection;

namespace Sector_dll.sdk
{
    class Vec4
    {

        public static object New(double x, double y, double z, double w)
        {
            if (SignatureManager.Vec4_Constructor_standalone == null)
                return null;
            return SignatureManager.Vec4_Constructor_standalone.Invoke(new object[] { x, y, z, w });
        }

        public static object New(object vec3)
        {
            if (SignatureManager.Vec4_Constructor_Vec3 == null)
                return null;
            return SignatureManager.Vec4_Constructor_Vec3.Invoke(new object[] { vec3 });
        }

        public static object Multiply(object vec4, object matrix4)
        {
            if (SignatureManager.Vec4_Multiply_Matrix4 == null)
                return null;
            return SignatureManager.Vec4_Multiply_Matrix4.Invoke(null, new object[] { vec4, matrix4 });
        }

        public Vec4(object vec4)
        {
            Type t = vec4.GetType();
            if (t == SignatureManager.Vec4.Type)
            {
                FieldInfo[] fields = t.GetFields();
                x = (double)fields[0].GetValue(vec4);
                y = (double)fields[1].GetValue(vec4);
                z = (double)fields[2].GetValue(vec4);
                w = (double)fields[3].GetValue(vec4);
            } else throw new NotSupportedException("can not cast from " + vec4.GetType().ToString() + " to vec4");
        }

        public object ToInternal()
        {
            return New(x, y, z, w);
        }

        public double x;

        public double y;

        public double z;

        public double w;

    }
}
