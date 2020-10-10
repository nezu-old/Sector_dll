using Sector_dll.util;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace sectorsedge.cheat.Hooks
{
    class WindowHandler
    {

        private static readonly uint WM_KEYDOWN = 0x0100;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IntPtr WindowProc(Func<object, IntPtr, uint, IntPtr, IntPtr, IntPtr> orig, object self, IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            //Log.Debug(uMsg + " - " + wParam.ToInt32());
            if (uMsg == WM_KEYDOWN && Menu.HandleKey((Keys)wParam.ToInt32()))
                return IntPtr.Zero; //An application should return zero if it processes this message.

            return orig(self, hwnd, uMsg, wParam, lParam);
        }
    }
}
