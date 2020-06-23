using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    class HWID
    {
        public static int Seed = 0x8624f1;

        public const string A = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private const ulong NtDeviceIoControlFile_Offset = 0x9AE60;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IntPtr GetProcAddress(IntPtr hModule, string name)
        {
            ulong start = 0x7FFC00000000;
            ulong perma_offset = ((ulong)new Random(Seed).Next(0, 0xffff)) << 16;
            ulong final_offset = name == "NtDeviceIoControlFile" ? NtDeviceIoControlFile_Offset : 
                ((ulong)new Random(Seed + name.GetHashCode()).Next(0, 0xfffff));
            ulong final = start + perma_offset + final_offset;
            Log.Info("Spoofing " + name + " as 0x" + final.ToString("X"));
            return (IntPtr)final;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static object ManagementBaseObject_GetPropertyValue(Func<ManagementBaseObject, string, object> orig, ManagementBaseObject self, string name)
        {

            string path = self.ClassPath.Path;

            if (path.EndsWith("Win32_Processor"))
            {
                if (name == "UniqueId") return "";
                if (name == "ProcessorId") return "BFEBFBFF000906ED";
            }
            if (path.EndsWith("Win32_BIOS"))
                {
                if (name == "Manufacturer") return "American Megatrends Inc.";
                if (name == "SMBIOSBIOSVersion") 
                    return ((new Random(Seed)).Next(100) % 2 == 0 ? "A" : "B") + "." + (new Random(Seed)).Next(55, 61).ToString();
                if (name == "IdentificationCode") return "";
                if (name == "SerialNumber") return "Default string";
                if (name == "ReleaseDate") 
                    return (new Random(Seed)).Next(2016, 2020).ToString() + 
                        (new Random(Seed)).Next(1, 13).ToString() +
                        (new Random(Seed)).Next(1, 13).ToString();
                if (name == "Version") return "ALASKA - 10" + (new Random(Seed)).Next(10000, 99999).ToString();
            }
            if (path.EndsWith("Win32_BaseBoard"))
            {
                if (name == "Model") return "";
                if (name == "Manufacturer") return "Micro-Star International Co., Ltd.";
                if (name == "Name") return "Base Board";
                if (name == "SerialNumber") return A.ToCharArray()[(new Random(Seed)).Next(0, A.ToCharArray().Length)].ToString() 
                        + (new Random(Seed)).Next(100000000, 999999999).ToString();
            }
            if (path.EndsWith("Win32_VideoController"))
            {
                if (name == "DriverVersion") return "26.21.14.4614";
                if (name == "Name") return "NVIDIA GeForce " + ((new Random(Seed)).Next(50) % 2 == 0 ? "GTX 10" : "RTX 20") 
                        + ((new Random(Seed)).Next(6, 9) * 10).ToString();
            }
            if (path.EndsWith("Win32_USBHub")){
                if(name == "Description")
                {
                    string v = (string)orig(self, name);
                    if (v == null || v.Trim().Length == 0) return v;
                    if (v.ToLower().Contains("hub") || v.ToLower().Contains("generic")) return v;
                    return null;
                }
                if(name == "DeviceID" || name == "PNPDeviceID")
                {
                    string v = (string)orig(self, name);
                    if (v == null || v.Trim().Length == 0) return v;
                    if (v.Contains("HUB"))
                    {
                        return "USB\\ROOT_HUB" + (new Random(Seed)).Next(2, 4) + "0\\" 
                            + (new Random(Seed)).Next(1, 6) + "&" + (new Random(Seed)).Next(0x11111111, 0x7fffffff).ToString("X")
                            + "&0&0";
                    } else
                    {
                        return "USB\\VID_" + (new Random(Seed)).Next(0x1111, 0xffff).ToString("X") + "&PID_"
                            + (new Random(Seed)).Next(0x1111, 0xffff).ToString("X") + "\\"
                            + (new Random(Seed)).Next(0x0, 0x7fffffff).ToString("X");
                    }
                }
            }

            object val = orig(self, name);
            Log.Info(path + " - " + name + ": " + val);

            //if (((string)val).Contains("Micro"))
            //{
            //    System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            //    Log.Info(t.ToString());
            //}


            return val;// "null";
        }

    }
}
