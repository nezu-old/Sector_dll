using MonoMod.RuntimeDetour;
using RGiesecke.DllExport;
using Sector_dll.cheat.Hooks;
using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Sector_dll.cheat
{
    public class Main
    {

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static IntPtr xd(IntPtr hModule, string name)//Func<IntPtr, string, IntPtr> orig, 
        //{
        //    //IntPtr xxx = orig(hModule, name);
        //    Log.Info("XDDDDDDDDDDDDDDDDD" + name);
        //    return IntPtr.Zero;
        //}

        [DllExport]
        public unsafe static void Entry()
        {
            AllocConsole();
            Log.enabled = true;
            Log.Info("Entry point called");
            Log.Info("Running in domain: " + AppDomain.CurrentDomain.FriendlyName);

            SignatureManager.FindSignatures();

            new Hook(SignatureManager.GClass49_vmethod_4, typeof(GClass49).GetMethod("vmethod_4"));
            //new Hook(SignatureManager.RequestHelper_POST, typeof(RequestHelper).GetMethod("POST"));
            //new Hook(SignatureManager.RequestHelper_GET, typeof(RequestHelper).GetMethod("GET"));

            new Hook(typeof(ManagementBaseObject).GetMethod("GetPropertyValue", BindingFlags.Public | BindingFlags.Instance), 
                typeof(HWID).GetMethod("ManagementBaseObject_GetPropertyValue"));
            new NativeDetour(SignatureManager.ConnectionHelper_GetProcAddress, typeof(HWID).GetMethod("GetProcAddress"));





            //new Hook(SignatureManager.GClass49_getPlayersToXray, typeof(GClass49).GetMethod("getPlayersToXray"));

            //Log.Info(SignatureManager.GenerateSig("#=zbJGgXq2WdIdgDEAoMXj69Yo="));

            //Assembly aassembly = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "sectorsedge");
            //MethodInfo mi = aassembly.GetType("#=zbJGgXq2WdIdgDEAoMXj69Yo=").GetMethod("#=zcclfLO$2ziIR");
            //new NativeDetour(mi, typeof(Main).GetMethod("xd"));


        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

    }
}
