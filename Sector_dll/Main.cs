using MonoMod.RuntimeDetour;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Sector_dll
{
    public class Main
    {
        public enum MapType
        {
            CF,
            ST,
            MC,
            AD,
            SS,
            CR,
            AR,
            IS,
            Custom,
            HL,
            AL
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static double SpeedModifier(Object self, MapType m)
        {
            return 5.0;
        }

        [DllExport]
        public static void Entry()
        {
            AllocConsole();
            Log.enabled = true;
            Log.Info("Entry point called");
            Log.Info("Running in domain: " + AppDomain.CurrentDomain.FriendlyName);

            foreach (Assembly aassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (aassembly.GetName().Name == "sectorsedge")
                {
                    foreach (Type t in aassembly.GetTypes())
                    {
                        if (t.IsClass && t.Name == "#=zG_pzwQYcjSt1ZBkrbMZaw5245nu3q8fdVQ==")
                        {
                            MethodInfo mi = t.GetMethod("#=zjmePDBhUHt0K");
                            Log.Info("0x" + mi.MethodHandle.GetFunctionPointer().ToString("X"));
                            try
                            {
                                IDetour hookTestMethodA = new Hook(
                                    mi,
                                    typeof(Main).GetMethod("SpeedModifier", BindingFlags.Static | BindingFlags.Public)
                                );
                                Log.Info(hookTestMethodA.IsValid.ToString());
                            }
                            catch (Exception ex)
                            {
                                Log.Info(ex.ToString());
                            }
                        }
                    }
                }
            }

        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

    }
}
