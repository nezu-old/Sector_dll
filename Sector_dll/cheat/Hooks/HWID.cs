using Sector_dll.util;
using System;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sector_dll.cheat.Hooks
{
    class HWID
    {
        public static int Seed = new Random().Next();

        public const string A = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        [StructLayout(LayoutKind.Sequential)]
        public struct MODULEINFO
        {
            public IntPtr lpBaseOfDll;
            public uint SizeOfImage;
            public IntPtr EntryPoint;
        }


        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("psapi.dll", SetLastError = true)]
        static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out MODULEINFO lpmodinfo, int cb);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IntPtr GetProcAddress(IntPtr hModule, string name)
        {
            MODULEINFO moduleInformation = new MODULEINFO();
            GetModuleInformation(GetCurrentProcess(), hModule, out moduleInformation, Marshal.SizeOf(moduleInformation));
            ulong start = (ulong)hModule;
            ulong perma_offset = (ulong)new Random(Seed).Next(0, (int)moduleInformation.SizeOfImage - 0xff);
            ulong final_offset = (ulong)new Random(Seed + name.GetHashCode()).Next(0, 0xff);
            ulong final = start + perma_offset + final_offset;
            Log.Info("Spoofing " + name + " as 0x" + final.ToString("X"));
            return (IntPtr)final;
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static object ManagementBaseObject_GetPropertyValue(Func<ManagementBaseObject, string, object> orig, ManagementBaseObject self, string name)
        {

            string path = self.ClassPath.Path;
            object val = orig(self, name);

            if (path.EndsWith("Win32_Processor"))
            {
                if (name == "UniqueId") val = "";
                if (name == "ProcessorId") val = "BFEBFBFF000906ED";
            }
            if (path.EndsWith("Win32_BIOS"))
                {
                if (name == "Manufacturer") val = "American Megatrends Inc.";
                if (name == "SMBIOSBIOSVersion")
                    val = ((new Random(Seed)).Next(100) % 2 == 0 ? "A" : "B") + "." + (new Random(Seed)).Next(55, 61).ToString();
                if (name == "IdentificationCode") val = "";
                if (name == "SerialNumber") val = "Default string";
                if (name == "ReleaseDate")
                    val = (new Random(Seed)).Next(2016, 2020).ToString() + 
                        (new Random(Seed)).Next(1, 13).ToString() +
                        (new Random(Seed)).Next(1, 13).ToString();
                if (name == "Version") val = "ALASKA - 10" + (new Random(Seed)).Next(10000, 99999).ToString();
            }
            if (path.EndsWith("Win32_BaseBoard"))
            {
                if (name == "Model") val = "";
                if (name == "Manufacturer") val = "Micro-Star International Co., Ltd.";
                if (name == "Name") val = "Base Board";
                if (name == "SerialNumber") val = A.ToCharArray()[(new Random(Seed)).Next(0, A.ToCharArray().Length)].ToString() 
                        + (new Random(Seed)).Next(100000000, 999999999).ToString();
            }
            if (path.EndsWith("Win32_VideoController"))
            {
                if (name == "DriverVersion") val = "26.21.14.4614";
                if (name == "Name") val = "NVIDIA GeForce " + ((new Random(Seed)).Next(50) % 2 == 0 ? "GTX 10" : "RTX 20") 
                        + ((new Random(Seed)).Next(6, 9) * 10).ToString();
            }
            if (path.EndsWith("Win32_USBHub")){
                if(name == "Description")
                {
                    string v = (string)orig(self, name);
                    if (v == null || v.Trim().Length == 0) val = v;
                    if (v.ToLower().Contains("hub") || v.ToLower().Contains("generic")) val = v;
                    val = "";
                }
                if(name == "DeviceID" || name == "PNPDeviceID")
                {
                    string v = (string)orig(self, name);
                    if (v == null || v.Trim().Length == 0) val = v;
                    if (v.Contains("HUB"))
                    {
                        val = "USB\\ROOT_HUB" + (new Random(Seed)).Next(2, 4) + "0\\" 
                            + (new Random(Seed)).Next(1, 6) + "&" + (new Random(Seed)).Next(0x11111111, 0x7fffffff).ToString("X")
                            + "&0&0";
                    } else
                    {
                        val = "USB\\VID_" + (new Random(Seed)).Next(0x1111, 0xffff).ToString("X") + "&PID_"
                            + (new Random(Seed)).Next(0x1111, 0xffff).ToString("X") + "\\"
                            + (new Random(Seed)).Next(0x0, 0x7fffffff).ToString("X");
                    }
                }
            }


            Log.Info(path + " - " + name + ": " + val);
            //if (((string)val).Contains("Micro"))
            //{
            //    System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            //    Log.Info(t.ToString());
            //}


            return val;// val;// "null";
        }

    }
}
