using Microsoft.Build.Utilities;
using Microsoft.Win32;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using RGiesecke.DllExport;
using Sector_dll.cheat.Hooks;
using Sector_dll.util;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sector_dll.cheat
{
    public class Main
    {


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string xd(Func<int, bool, string>orig, int i, bool b)//Func<IntPtr, string, IntPtr> orig, 
        {
            string s = orig(i, b);
            //if (s.ToLower().Contains("head"))
            {
                Log.Info("I: " + i + " S: " + s);
                //Log.Dump(Tracing.file, "New string: " + s);
                //StackTrace t = new StackTrace();
                //Log.Info(t.ToString());

                //using (FileStream fileStream = new FileStream("string_order.txt", FileMode.Append))
                //{
                //    using(StreamWriter sw = new StreamWriter(fileStream))
                //    {
                //        sw.WriteLine(s);
                //    }
                //}

            }
            return s;
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader0([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 0)] string[] a) { }
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader1([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 1)] string[] a) { }
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader2([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 2)] string[] a) { }
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader3([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 3)] string[] a) { }
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader4([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 4)] string[] a) { }
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader5([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 5)] string[] a) { }
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader6([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 6)] string[] a) { }
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader7([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 7)] string[] a) { }
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader8([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 8)] string[] a) { }
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        public static void MainLoader9([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 9)] string[] a) { }

        [DllExport()]
        public unsafe static void Entry()
        {
            AllocConsole();
            Log.enabled = true;
            Log.Info("Entry point called");
            Log.Info("Running from: " + Assembly.GetExecutingAssembly().Location);
            //Log.Info("Running in domain: " + AppDomain.CurrentDomain.FriendlyName);
            //Log.Info("Domain base dir: " + AppDomain.CurrentDomain.BaseDirectory);
            Log.Info("Working directory: " + Directory.GetCurrentDirectory());

            //Console.Read();

            new Hook(typeof(File).GetMethod("Exists"), typeof(Antycheat).GetMethod("FileExists"));
            new Hook(typeof(Directory).GetMethod("Exists"), typeof(Antycheat).GetMethod("DirectoryExists"));
            Antycheat.origFileSystemEnumerableIterator = new NativeDetour(typeof(Directory).Assembly.GetTypes()
                .First(x => x.Name.Contains("FileSystemEnumerableIterator")).MakeGenericType(typeof(object))
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.GetParameters().Length == 6), 
                typeof(Antycheat).GetMethod("FileSystemEnumerableIterator"), new NativeDetourConfig() { SkipILCopy = true })
                .GenerateTrampoline<Antycheat.FileSystemEnumerableIteratorDelegate>();
            //new Hook(typeof(FileStream).GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Antycheat).GetMethod("FileStreamInit"));
            Antycheat.origAppDomainnGetAssemblies = new NativeDetour(typeof(AppDomain).GetMethod("nGetAssemblies", 
                BindingFlags.Instance | BindingFlags.NonPublic), typeof(Antycheat).GetMethod("AppDomainnGetAssemblies"), 
                new NativeDetourConfig() { SkipILCopy = true } ).GenerateTrampoline<Antycheat.AppDomainnGetAssembliesDeleghate>();

            try
            {
                int steamId = (int)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\ActiveProcess", "ActiveUser", new Random().Next());
                HWID.Seed = steamId;
                Log.Debug("Set hwid seed to: " + steamId);
            } 
            catch (Exception ex)
            {
                Log.Danger("Failed to get current steamid, exiting to prevent detection");
                Log.Danger(ex.ToString());
                Log.Danger("Press any key to exit");
                Console.Read();
                Environment.Exit(10);
            }

            //while (!Debugger.IsAttached) Thread.Sleep(100);
            
            Assembly assembly = Assembly.Load("sectorsedge");

            string[] a = Environment.GetCommandLineArgs();
            string[] args = new string[a.Length - 1];
            Array.Copy(a, 1, args, 0, a.Length - 1);
            Log.Info("args: " + string.Join(" ", args));

            //Log.Info(SignatureManager.GenerateSig("#=z1q5LvvTvJVHL95mpiAw3sxdwWtd5ROgm6g==")); Console.Read();

            try
            {
                if (!SignatureManager.FindSignatures(assembly))
                    throw new Exception("Failed to resolve all types/methods/fields");
                
                new Hook(SignatureManager.RequestHelper_POST, typeof(RequestHelper).GetMethod("POST"));
                new Hook(SignatureManager.RequestHelper_GET, typeof(RequestHelper).GetMethod("GET"));
                
                new Hook(SignatureManager.GClass49_Base_Base_Draw, typeof(GClass49).GetMethod("vmethod_4"));
                new Hook(SignatureManager.LocalPlayer_Update, typeof(Player).GetMethod("Update"));
                //new Hook(SignatureManager.PlayerBase_RecoilMod, typeof(Player).GetMethod("RecoilMod"));
                //new Hook(SignatureManager.Helper_CurrentBloom, typeof(Helper).GetMethod("CurrentBloom"));

                new Hook(typeof(ManagementBaseObject).GetMethod("GetPropertyValue", BindingFlags.Public | BindingFlags.Instance),
                    typeof(HWID).GetMethod("ManagementBaseObject_GetPropertyValue"));
                
                //new NativeDetour(SignatureManager.Helper1_GetProcAddress, typeof(HWID).GetMethod("GetProcAddress"));

                new Detour(typeof(Environment).GetMethod("get_MachineName", BindingFlags.Public | BindingFlags.Static),
                    typeof(HWID).GetMethod("get_MachineName"));
                new Detour(typeof(Environment).GetMethod("get_UserName", BindingFlags.Public | BindingFlags.Static),
                    typeof(HWID).GetMethod("get_UserName"));
                new Detour(typeof(File).GetMethod("GetCreationTimeUtc", BindingFlags.Public | BindingFlags.Static),
                    typeof(HWID).GetMethod("GetCreationTimeUtc"));
                new Detour(typeof(Directory).GetMethod("GetCreationTimeUtc", BindingFlags.Public | BindingFlags.Static),
                    typeof(HWID).GetMethod("GetCreationTimeUtc"));

                MethodInfo[] eac_funcs = assembly.GetType("#=z5Yqy3S3egHnqI0lLeEnH1ZMw$GJCKrFMNkluyr8=")
                    .GetNestedType("#=zJyN_ZSM9809$", BindingFlags.NonPublic).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

                foreach (MethodInfo mi in eac_funcs)
                {
                    new NativeDetour(mi, typeof(EAC).GetMethod(mi.GetCustomAttribute<DllImportAttribute>().EntryPoint));
                }

                foreach (MethodInfo method in SignatureManager.RegQueryValueEx)
                    HWID.oRegQueryValueEx = new NativeDetour(method, typeof(HWID).GetMethod("RegQueryValueEx"))
                        .GenerateTrampoline<HWID.RegQueryValueExDelegate>();
                new NativeDetour(SignatureManager.DiscordCreate, typeof(HWID).GetMethod("DiscordCreate"));

                //Func<Func<object, int>, object, int> fuseXD = (Func<object, int> orig, object self) => {
                //    //int ret = orig(self);
                //    //Log.Info(ret);
                //    return 3000;// ret;
                //};

                //new Hook(SignatureManager.Grenade.Type.GetMethod("#=zsz0Iy4N_1cEc"), fuseXD.Method, new object());

                //throw new Exception("xd");

            }
            catch (Exception e)
            {
                Log.Danger(e.ToString());
                Log.Danger("Press any key to exit");
                Console.ReadLine();
                Environment.Exit(10);
            }


            //return;
            //MethodInfo mi = assembly.GetType("#=qdoFfi5oHiWQ_F2sP8WpOBcxXYtKebPWkOJgS_W$6XCc=")
            //    .GetMethod("#=zmeMzoCRvmfuINxK3$qUENbA=", BindingFlags.NonPublic | BindingFlags.Static);

            //new Hook(mi, typeof(Main).GetMethod("xd"));

            //Util.DumpStrings(assembly, mi);

            //Environment.Exit(10);

            //Util.DumpShit(assembly);

            //Log.Danger((string)mi.Invoke(null, new object[] { -2001122674, true }));
            //Console.Read();

            ///Detour mainDetour = new Detour(assembly.EntryPoint, typeof(Main).GetMethod("MainHook")); //detour main to dummy function
            //Tracing.ApplyHooks();

            typeof(AppDomain).GetMethod("nExecuteAssembly", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(AppDomain.CurrentDomain, new object[] { assembly, args });

            Console.ReadLine();

            //mainDetour.Dispose(); // rstore it

            //for(int i = 0; i < 10; i ++)
            //    new NativeDetour(typeof(Main).GetMethod("MainLoader" + i), assembly.EntryPoint);

            //try
            //{
            //    IntPtr xd = GetModuleHandle(Path.GetFileName(Assembly.GetExecutingAssembly().Location));
            //    if(xd != IntPtr.Zero)
            //    {
            //        IntPtr mainLoader = GetProcAddress(xd, "MainLoader" + args.Length) ;

            //        IntPtr data = Marshal.AllocHGlobal(1024 * 10);
            //        int offset = Marshal.SizeOf<IntPtr>() * args.Length;
            //        IntPtr lastAddr = IntPtr.Add(data, offset);
            //        for(int i = 0; i < args.Length; i++)
            //        {
            //            byte[] s = Encoding.ASCII.GetBytes(args[i] + "\0");
            //            Marshal.Copy(s, 0, lastAddr, s.Length);
            //            byte[] addy = BitConverter.GetBytes(lastAddr.ToInt64());
            //            Marshal.Copy(addy, 0, new IntPtr(data.ToInt64() + (i * Marshal.SizeOf<IntPtr>())), addy.Length);
            //            lastAddr = new IntPtr(lastAddr.ToInt64() + s.Length);
            //        }
            //        //Marshal.GetDelegateForFunctionPointer<FakeMainDelegate>(mainLoader)(data);
            //        //Log.Danger("Press enter to start");
            //        //Console.Read();

            //        Tracing.ApplyHooks();
            //        //while (!Debugger.IsAttached) Thread.Sleep(10);

            //        Console.ReadLine();

            //        IntPtr thread = CreateThread(UIntPtr.Zero, 0, mainLoader, data, 0, IntPtr.Zero);
            //        WaitForSingleObject(thread, 0xFFFFFFFF);// INFINITE 

            //        Log.Danger("Main ended, exiting");
            //        Environment.Exit(0);

            //    }
            //} 
            //catch(Exception ex)
            //{
            //    Log.Danger(ex.ToString());
            //}

        }

        [DllImport("kernel32.dll", SetLastError= true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        [DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private unsafe static extern IntPtr CreateThread(
            UIntPtr lpThreadAttributes,
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FakeMainDelegate(IntPtr data);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void MainHook(string[] args)
        {
            Log.Info("Main prevented: " + string.Join(" ", args));
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

    }
}
