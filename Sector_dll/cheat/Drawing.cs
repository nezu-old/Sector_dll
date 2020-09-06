using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sector_dll.cheat
{
    class Drawing
    {

        private static List<object>[,] NormalBones = new List<object>[30, 2];

        [StructLayout(LayoutKind.Sequential)]
        public struct DrawingFunctions {

            public enum TextAlignment : int
            {
                ALIGN_TOP = 1,
                ALIGN_BOTTOM = 2,
                ALIGN_LEFT = 4,
                ALIGN_RIGHT = 8,
                ALIGN_VCENTER = 16,
                ALIGN_HCENTER = 32,
                ALIGN_CENTER = ALIGN_VCENTER | ALIGN_HCENTER,
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawMenuDelegate(ref Config.Settings settings);
            
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawRectDelegate(int x, int y, int w, int h, int t, uint color);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawFilledRectDelegate(int x, int y, int w, int h, uint color);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawLineDelegate(float x1, float y1, float x2, float y2, float t, uint color);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawTextDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string text, float x, float y, float size = 20, uint color = 0xFFFFFFFF,
                [MarshalAs(UnmanagedType.I4)] TextAlignment alignment = 0);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawTextSmallDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string text, float x, float y, uint color = 0xFFFFFFFF,
                [MarshalAs(UnmanagedType.I4)] TextAlignment alignment = 0);

            public DrawMenuDelegate DrawMenu;

            public DrawRectDelegate DrawRect;

            public DrawFilledRectDelegate DrawFilledRect;

            public DrawLineDelegate DrawLine;

            public DrawTextDelegate DrawText;

            public DrawTextSmallDelegate DrawTextSmall;

        }

        public static void DrawCallback(ref DrawingFunctions d)
        {
            d.DrawMenu(ref Config.settings);

            if (Config.settings.debug6 > 0) Log.Info("Frame");

            if (GameManager.instance.IsAlive && GameManager.instance.Target.GetType().BaseType == SignatureManager.GClass49.Type.BaseType)
            {
                object gm = GameManager.instance.Target;

                //d.DrawFilledRect(20, 20, 10, 10, 0xFF0000FF);

                try
                {
                    ESP.DrawPlayerEsp(d);

                    ESP.DrawProjectiles(d);
                }
                catch (Exception ex) {
                    Log.Danger(ex);
                }

            }
        }

    }
}
