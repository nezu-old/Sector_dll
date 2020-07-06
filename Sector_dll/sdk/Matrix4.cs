using Sector_dll.cheat;
using System.Windows.Forms;

namespace Sector_dll.sdk
{
    class Matrix4
    {
        public static object New(double d1, double d2, double d3, double d4, double d5, double d6, double d7, double d8,
            double d9, double d10, double d11, double d12, double d13, double d14, double d15, double d16)
        {
            if (SignatureManager.Matrix4_Constructor == null)
                return null;
            return SignatureManager.Matrix4_Constructor.Invoke(new object[] { d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16 });
        }

        public static object New(double x, double y, double z)
        {
            return New(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, x, y, z, 1.0);
        }

        public static object Multiply(object matrix1, object matrix2)
        {
            if (SignatureManager.Matrix4_Multiply_Matrix4 == null)
                return null;
            return SignatureManager.Matrix4_Multiply_Matrix4.Invoke(null, new object[] { matrix1, matrix2 });
        }

        public static object Generate(double fov, double ratio, double near, double far)
        {
            if (SignatureManager.Matrix4_Generate == null)
                return null;
            return SignatureManager.Matrix4_Generate.Invoke(null, new object[] { fov, ratio, near, far });
        }

    }
}
