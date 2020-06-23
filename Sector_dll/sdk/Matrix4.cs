using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class Matrix4
    {

        public static object Multiply(object matrix1, object matrix2)
        {
            if (SignatureManager.Matrix4_Multiply_Matrix4 == null)
                return null;
            return SignatureManager.Matrix4_Multiply_Matrix4.Invoke(null, new object[] { matrix1, matrix2 });
        }

    }
}
