using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class Rect
    {

        public static object New(double x, double y, double w, double h)
        {
            if (SignatureManager.Rect_Constructor_standalone == null)
                return null;
            return SignatureManager.Rect_Constructor_standalone.Invoke(new object[] { x, y, w, h });
        }

        public static object New(object pos, object size)
        {
            if (SignatureManager.Rect_Constructor_vec2 == null)
                return null;
            return SignatureManager.Rect_Constructor_vec2.Invoke(new object[] { pos, size});
        }

    }
}
