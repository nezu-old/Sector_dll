using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class Color
    {

        public static object New(uint color){
            if (SignatureManager.Color_Constructor_uint == null) 
                return null;
            return SignatureManager.Color_Constructor_uint.Invoke(new object[] { color });
        }

        public static object New(byte r, byte g, byte b, byte a = 255)
        {
            if (SignatureManager.Color_Constructor_byte == null)
                return null;
            return SignatureManager.Color_Constructor_byte.Invoke(new object[] { r, g, b, a });
        }

        public static object New(float r, float g, float b, float a = 1f)
        {
            if (SignatureManager.Color_Constructor_uint == null)
                return null;
            return SignatureManager.Color_Constructor_uint.Invoke(new object[] { r, g, b, a });
        }

    }
}
