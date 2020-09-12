using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    unsafe class GL
    {
        public delegate bool SwapBuffersDelegate(IntPtr hdc);

        public static SwapBuffersDelegate SwapBuffersOrig;

        static bool init_ok = false;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool SwapBuffers(IntPtr hdc)
        {
            if(wglGetCurrentContext() != IntPtr.Zero)
            {
                if (!init_ok)
                {
                    OpenGL.InitOpenGL();
                    init_ok = true;
                } 
                else
                {

                }

            }
            //Log.Debug("SwapBuffers");
            return SwapBuffersOrig(hdc);
        }

        [DllImport("opengl32.dll", EntryPoint = "wglGetProcAddress")]
        public static extern IntPtr wglGetProcAddress([MarshalAs(UnmanagedType.LPStr)] [In] string name);

        [DllImport("opengl32.dll", EntryPoint = "wglGetCurrentContext", SetLastError = true)]
        public static extern IntPtr wglGetCurrentContext();

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    }
}
