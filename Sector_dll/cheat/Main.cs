using MonoMod.RuntimeDetour;
using RGiesecke.DllExport;
using Sector_dll.cheat.Hooks;
using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Sector_dll.cheat
{
    public class Main
    {

        

        [DllExport]
        public static void Entry()
        {
            AllocConsole();
            Log.enabled = true;
            Log.Info("Entry point called");
            Log.Info("Running in domain: " + AppDomain.CurrentDomain.FriendlyName);

            SignatureManager.FindSignatures();

            new Hook(SignatureManager.GClass49_vmethod_4, typeof(GClass49).GetMethod("vmethod_4", BindingFlags.Public | BindingFlags.Static));



            //Log.Info(SignatureManager.GenerateSig("#=zLptqDxOwQsb72TGFBzgTqAM1dPnD"));

        }


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

    }
}
