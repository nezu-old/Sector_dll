using Sector_dll.cheat;

namespace Sector_dll.sdk
{
    class Font
    {

        public static object New(string name, double size, int weight)
        {
            if (SignatureManager.Font_Constructor == null)
                return null;
            return SignatureManager.Font_Constructor.Invoke(new object[] { name, size, weight });
        }

        public const string GameFont = "Bebas Neue";

    }
}
