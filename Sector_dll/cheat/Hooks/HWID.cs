using Microsoft.Win32;
using Mono.Cecil;
using Sector_dll.util;
using System;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sector_dll.cheat.Hooks
{
    class HWID
    {
        public static int Seed = 0;// new Random().Next();

        public static char[] A = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public static char[] A1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890".ToCharArray();

        public static string randomStr(char[] c, int len)
        {
            StringBuilder sb = new StringBuilder(len);
            Random r = new Random(Seed + c.Length);
            for (int i = 0; i < len; i++)
            {
                sb.Append(c[r.Next(c.Length)]);
            }
            return sb.ToString();
        }

        public static string randomGpu()
        {
            Random r = new Random(Seed + 69);
            if(r.Next(100) > 50)
            {
                int gen = r.Next(6, 12);
                if (gen == 11) 
                    gen = 20;//cuz nvidia big brain 10 + 1 = 20 XD
                string series = gen == 20 ? "RTX" : r.Next(2) == 0 && gen < 10 ? "GT" : "GTX";
                int version = r.Next(6, 9) * 10;
                return "NVIDIA GeForce " + series + " " + gen.ToString() + version.ToString();
            }
            else
            {
                int gen = r.Next(9);
                int x1 = r.Next(7);
                int x2 = r.Next(8);
                return "Intel(R) HD Graphics " + (gen != 0 ? gen.ToString() : "") + x1.ToString() + x2.ToString() + "0";
            }
        }

        public static string intelCpuId()
        {
            switch(new Random(Seed + 3).Next(3))
            {
                case 0: return "BFEBFBFF000306D4";
                case 1: return "BFEBFBFF000906E9";
                case 2: return "BFEBFBFF000306A9";
            }
            return "BFEBFBFF00030669"; //idk XD
        }

        public static string[] mobo_m = new string[] 
        { 
            "Insyde", 
            "Dell Inc.", 
            "Micro-Star International Co., Ltd.",
            "Hewlett-Packard",
            "HP", //LOL they use both XD
        };

        public static string[] mobo_p = new string[] // prefixes fo shit
        {
            "Insyde Version ",
            "Dell - ",
            "ALASKA - ",
            "Hewlett-Packard - ",
            "HPQOEM - ", //LOL they use both XD
        };

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

        [DllImport("ntdll.dll", EntryPoint = "NtQueryKey")]
        static extern uint NtQueryKey(IntPtr key, int keyInformationClass, IntPtr keyInformation, int len, ref int resultLength);

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
                if (name == "ProcessorId") val = intelCpuId();
            }
            if (path.EndsWith("Win32_BIOS"))
                {
                if (name == "Manufacturer") val = mobo_m[new Random(Seed).Next(mobo_m.Length)];
                if (name == "SMBIOSBIOSVersion")
                    val = A[new Random(Seed).Next(A.Length)] + (new Random(Seed)).Next(20, 61).ToString();
                if (name == "IdentificationCode") val = "";
                if (name == "SerialNumber") val = randomStr(A1, new Random(Seed).Next(5, 10));
                if (name == "ReleaseDate")
                    val = (new Random(Seed)).Next(2016, 2020).ToString() + 
                        (new Random(Seed)).Next(1, 13).ToString() +
                        (new Random(Seed)).Next(1, 13).ToString();
                if (name == "Version") val = mobo_p[new Random(Seed).Next(mobo_m.Length)] +
                        (new Random(Seed)).Next(10000, 99999).ToString(new Random(Seed - 1).Next(101) % 2 == 0 ? "X" : "D");
            }
            if (path.EndsWith("Win32_BaseBoard"))
            {
                if (name == "Model") val = "";
                if (name == "Manufacturer") val = mobo_m[new Random(Seed).Next(mobo_m.Length)];
                if (name == "Name") val = "Base Board";
                if (name == "SerialNumber") val = randomStr(A1, new Random(Seed).Next(4, 16));
            }
            if (path.EndsWith("Win32_VideoController"))
            {
                if (name == "DriverVersion") val = new Random(Seed).Next(10, 24) + "." + new Random(Seed).Next(18, 22) + 
                        "." + new Random(Seed).Next(10, 50) + "." + new Random(Seed).Next(1000, 5000);
                if (name == "Name") val = randomGpu();
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

        public delegate uint RegQueryValueExDelegate(IntPtr key, string valueName, int reserved, out int type, IntPtr data, ref int dataSize);

        public static RegQueryValueExDelegate oRegQueryValueEx;

        private static readonly byte[] key_template = new byte[]
        {
            0xFF, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0xFF, 0x00, 0x00,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF,
            0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF,
            0xFF, 0xFF
        };

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static uint RegQueryValueEx(IntPtr key, string valueName, int reserved, out int type, IntPtr data, ref int dataSize)
        {
            try
            {
                int len = 0;
                NtQueryKey(key, 3, IntPtr.Zero, 0, ref len);
                IntPtr name_data = Marshal.AllocHGlobal(len);
                byte[] bytes = new byte[len];
                if (NtQueryKey(key, 3, name_data, len, ref len) == 0)
                {
                    Marshal.Copy(name_data, bytes, 0, bytes.Length);
                    byte[] bytes2 = new byte[bytes.Length - 4];
                    Array.Copy(bytes, 4, bytes2, 0, bytes2.Length);
                    string key_name = Encoding.Unicode.GetString(bytes2);
                    string full_name = key_name + "\\" + valueName;
                    Log.Danger("RegQueryValueEx(" + full_name + ")");
                    if(full_name.ToLower().EndsWith(@"Microsoft\Windows NT\CurrentVersion\DigitalProductId".ToLower()))
                    {
                        Random r = new Random(Seed);
                        byte[] fake_key = new byte[key_template.Length];
                        for(int i = 0; i < fake_key.Length; i++)
                        {
                            if (key_template[i] == 0xFF)
                                fake_key[i] = (byte)r.Next(0xFF);
                            else
                                fake_key[i] = 0;
                        }
                        type = 0x69; // to lazy to google the actual type ( ͡• ͜ʖ ͡• )
                        Marshal.Copy(fake_key, 0, data, Math.Min(fake_key.Length, dataSize)); //to lazy to throw XD
                        dataSize = fake_key.Length;
                        Log.Danger("windows key spoofed!");
                        return 0U;
                    }
                }
                else 
                    throw new Exception("NtQueryKey failed!");
            }
            catch (Exception ex)
            {
                Log.Danger("RegQueryValueEx failed: " + ex.ToString());
            }
            return oRegQueryValueEx(key, valueName, reserved, out type, data, ref dataSize);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string get_MachineName()
        {
            Random r = new Random(Seed);
            string s = "DESKTOP-" + A[r.Next(A.Length)] + A[r.Next(A.Length)] + r.Next(999).ToString()
                + A[r.Next(A.Length)] + A[r.Next(A.Length)];
            Log.Danger("Spoffed get_MachineName as: " + s);
            return s;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string get_UserName()
        {
            string name;
            try
            {
                name = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "LastGameNameUsed", "admin");
            } 
            catch(Exception)
            {
                name = "admin";
            }
            Log.Danger("Spoffed get_UserName as: " + name);
            return name;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static DateTime GetCreationTimeUtc(string path)
        {
            Random r = new Random(Seed);
            DateTime time = DateTime.UtcNow;
            if(path == Environment.GetFolderPath(Environment.SpecialFolder.Windows))
            {
                time = time.Subtract(new TimeSpan(r.Next(1000), r.Next(24), r.Next(60), r.Next(60)));
            }
            else if(path == Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
            { 
                time = time.Subtract(new TimeSpan(r.Next(1000), r.Next(24), r.Next(60), r.Next(60)))
                    .AddMinutes(r.Next(30, 80));
            }
            else
            { 
                time = time.Subtract(new TimeSpan(r.Next(200), r.Next(20), r.Next(50), r.Next(10)));
            }
            Log.Danger("Spoofing File.GetCreationTimeUtc(" + path + ") as " + time.ToString()); 
            return time;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static object DiscordCreate(UInt32 version, ref object createParams, out IntPtr manager)
        {
            manager = IntPtr.Zero;
            object ret = Enum.ToObject(SignatureManager.DiscordCreate.ReturnType, 26); // Dircord.Resul.NotInstalled
            Log.Danger("Spoofing discord init as: " + ret.ToString());
            return ret;
        }

    }
}
