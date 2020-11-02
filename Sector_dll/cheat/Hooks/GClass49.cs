using Sector_dll.sdk;
using System;
using System.Runtime.CompilerServices;

namespace Sector_dll.cheat.Hooks
{
    class GClass49
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void vmethod_4(Action<object, object> orig, object self, object p1)
        {
            if(self.GetType().BaseType == SignatureManager.GClass49.Type.BaseType)
            {
                //Log.Debug(self.GetType());
                GameManager.instance.Target = self;
                GameManager.NewFrame(self);
            }
            else
            {
                GameManager.instance.Target = null;
            }
         
            orig(self, p1);
        }
    }
}
