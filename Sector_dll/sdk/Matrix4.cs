using Sector_dll.cheat;
using System;
using System.CodeDom;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Sector_dll.sdk
{
    class Matrix4
    {
        public static object New(double d1, double d2, double d3, double d4, double d5, double d6, double d7, double d8,
            double d9, double d10, double d11, double d12, double d13, double d14, double d15, double d16)
        {
            return SignatureManager.Matrix4_Constructor.Invoke(new object[] { d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16 });
        }

        public static object New(double x, double y, double z)
        {
            return New(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, x, y, z, 1.0);
        }

        public static object Multiply(object matrix1, object matrix2)
        {
            return SignatureManager.Matrix4_Multiply_Matrix4.Invoke(null, new object[] { matrix1, matrix2 });
        }

        public static object Generate(double fov, double ratio, double near, double far)
        {
            return SignatureManager.Matrix4_Generate.Invoke(null, new object[] { fov, ratio, near, far });
        }

        public Matrix4(double d1, double d2, double d3, double d4, double d5, double d6, double d7, double d8,
            double d9, double d10, double d11, double d12, double d13, double d14, double d15, double d16)
        {
            M11 = d1;
            M12 = d2;
            M13 = d3;
            M14 = d4;
            M21 = d5;
            M22 = d6;
            M23 = d7;
            M24 = d8;
            M31 = d9;
            M32 = d10;
            M33 = d11;
            M34 = d12;
            M41 = d13;
            M42 = d14;
            M43 = d15;
            M44 = d16;
        }

        public Matrix4(object Matrix4F)
        {
            if(Matrix4F_fields == null)
                Matrix4F_fields = Matrix4F.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            M11 = (float)Matrix4F_fields[0].GetValue(Matrix4F);
            M12 = (float)Matrix4F_fields[1].GetValue(Matrix4F);
            M13 = (float)Matrix4F_fields[2].GetValue(Matrix4F);
            M14 = (float)Matrix4F_fields[3].GetValue(Matrix4F);
            M21 = (float)Matrix4F_fields[4].GetValue(Matrix4F);
            M22 = (float)Matrix4F_fields[5].GetValue(Matrix4F);
            M23 = (float)Matrix4F_fields[6].GetValue(Matrix4F);
            M24 = (float)Matrix4F_fields[7].GetValue(Matrix4F);
            M31 = (float)Matrix4F_fields[8].GetValue(Matrix4F);
            M32 = (float)Matrix4F_fields[9].GetValue(Matrix4F);
            M33 = (float)Matrix4F_fields[10].GetValue(Matrix4F);
            M34 = (float)Matrix4F_fields[11].GetValue(Matrix4F);
            M41 = (float)Matrix4F_fields[12].GetValue(Matrix4F);
            M42 = (float)Matrix4F_fields[13].GetValue(Matrix4F);
            M43 = (float)Matrix4F_fields[14].GetValue(Matrix4F);
            M44 = (float)Matrix4F_fields[15].GetValue(Matrix4F);
        }

        public static Vec3 operator *(Matrix4 m, Vec3 v)
        {
            return new Vec3(
                v.x * m.M11 + v.y * m.M21 + v.z * m.M31 + m.M41,
                v.x * m.M12 + v.y * m.M22 + v.z * m.M32 + m.M42,
                v.x * m.M13 + v.y * m.M23 + v.z * m.M33 + m.M43
                );
        }

        public static Matrix4 operator* (Matrix4 m1, Matrix4 m2)
        {
            return new Matrix4(
                m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31 + m1.M14 * m2.M41,
                m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32 + m1.M14 * m2.M42,
                m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33 + m1.M14 * m2.M43,
                m1.M11 * m2.M14 + m1.M12 * m2.M24 + m1.M13 * m2.M34 + m1.M14 * m2.M44,
                m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31 + m1.M24 * m2.M41,
                m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32 + m1.M24 * m2.M42,
                m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33 + m1.M24 * m2.M43,
                m1.M21 * m2.M14 + m1.M22 * m2.M24 + m1.M23 * m2.M34 + m1.M24 * m2.M44,
                m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31 + m1.M34 * m2.M41,
                m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32 + m1.M34 * m2.M42,
                m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33 + m1.M34 * m2.M43,
                m1.M31 * m2.M14 + m1.M32 * m2.M24 + m1.M33 * m2.M34 + m1.M34 * m2.M44,
                m1.M41 * m2.M11 + m1.M42 * m2.M21 + m1.M43 * m2.M31 + m1.M44 * m2.M41,
                m1.M41 * m2.M12 + m1.M42 * m2.M22 + m1.M43 * m2.M32 + m1.M44 * m2.M42,
                m1.M41 * m2.M13 + m1.M42 * m2.M23 + m1.M43 * m2.M33 + m1.M44 * m2.M43,
                m1.M41 * m2.M14 + m1.M42 * m2.M24 + m1.M43 * m2.M34 + m1.M44 * m2.M44
                );
        }

        public static Matrix4 CreateRotationX(double rot)
        {
            double cos = Math.Cos(rot);
		    double sin = Math.Sin(rot);
		    return new Matrix4(1.0, 0.0, 0.0, 0.0, 0.0, cos, sin, 0.0, 0.0, -sin, cos, 0.0, 0.0, 0.0, 0.0, 1.0);
        }

        public static Matrix4 CreateRotationY(double rot)
	    {
		    double cos = Math.Cos(rot);
		    double sin = Math.Sin(rot);
		    return new Matrix4(cos, 0.0, -sin, 0.0, 0.0, 1.0, 0.0, 0.0, sin, 0.0, cos, 0.0, 0.0, 0.0, 0.0, 1.0);
	    }

        public static Matrix4 CreateTranslation(Vec3 vec)
	    {
		    return new Matrix4(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, vec.x, vec.y, vec.z, 1.0);
	    }

        private static FieldInfo[] Matrix4F_fields;

        public double M11;

		public double M12;

		public double M13;

		public double M14;

		public double M21;

		public double M22;

		public double M23;

		public double M24;

		public double M31;

		public double M32;

		public double M33;

		public double M34;

		public double M41;

		public double M42;

		public double M43;

		public double M44;

	}
}
