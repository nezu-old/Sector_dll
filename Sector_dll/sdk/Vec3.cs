using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class Vec3
    {

        public static object New(double x, double y, double z)
        {
            if (SignatureManager.Vec3_Constructor_double == null)
                return null;
            return SignatureManager.Vec3_Constructor_double.Invoke(new object[] { x, y, z });
        }

        public Vec3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(object vec3)
        {
            Type t = vec3.GetType();
            if (t == SignatureManager.Vec3)
            {
                FieldInfo[] fields = t.GetFields();
                x = (double)fields[0].GetValue(vec3);
                y = (double)fields[1].GetValue(vec3);
                z = (double)fields[2].GetValue(vec3);
            }
            else throw new NotSupportedException("can not cast from " + t.ToString() + " to vec3");
        }

        public object ToInternal()
        {
            return New(x, y, z);
        }

        public double x;

        public double y;

        public double z;

    }
}
