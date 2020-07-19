using Sector_dll.cheat;

namespace Sector_dll.sdk
{
    class Size
    {

        public static object New(double x, double y)
        {
            return SignatureManager.Size_Constructor.Invoke(new object[] { x, y });
        }

    }
}
