using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sector_dll.cheat.Hooks
{
    class GClass49
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void vmethod_4(Action<object, object> orig, object self, object p1)
        {
            GameManager.instance.Target = self;
            GameManager.NewFrame(self);
            //throw new FileNotFoundException("XD");

            //object localPlayer = GameManager.GetLocalPLayer(self);
         
            orig(self, p1);
        }
    }
}
