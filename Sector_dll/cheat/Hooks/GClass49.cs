using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    class GClass49
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void vmethod_4(Action<Object, Object> orig, Object self, Object p1)
        {
            object rect = SignatureManager.Rect_Constructor_standalone.Invoke(new object[] { 50, 50, 100, 100 });

            object color = SignatureManager.Color_Constructor_byte.Invoke(new object[] { (byte)0, (byte)255, (byte)0, (byte)255 });

            SignatureManager.DrawingHelper_DrawRect.Invoke(null, new object[] { p1, rect, 2.0, color });

            var xd = (SignatureManager.GClass49_player_list.GetValue(self) as IEnumerable<object>).Cast<object>().ToList();

            Log.Info(xd.Count.ToString());

            orig(self, p1);
        }
    }
}
