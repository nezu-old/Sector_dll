using Sector_dll.cheat;

namespace Sector_dll.sdk
{
    class Rect
    {

        public static object New(double x, double y, double w, double h)
        {
            return SignatureManager.Rect_Constructor_standalone.Invoke(new object[] { x, y, w, h });
        }

        public static object New(object pos, object size)
        {
            return SignatureManager.Rect_Constructor_vec2.Invoke(new object[] { pos, size});
        }

    }
}
