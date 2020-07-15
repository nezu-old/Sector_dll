using Microsoft.Build.Utilities;
using Microsoft.Win32;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using RGiesecke.DllExport;
using Sector_dll.cheat.Hooks;
using Sector_dll.util;
using Steamworks;
using System;
using System.CodeDom;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
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
            //if (s.Contains("OpenGL3 init"))
            {
                //Log.Info("I: " + i + " S: " + s);
                //StackTrace t = new StackTrace();
                //Log.Info(t.ToString());
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
            //Log.Info("Running from: " + Assembly.GetExecutingAssembly().Location);
            //Log.Info("Running in domain: " + AppDomain.CurrentDomain.FriendlyName);
            //Log.Info("Domain base dir: " + AppDomain.CurrentDomain.BaseDirectory);
            Log.Info("Working directory: " + Directory.GetCurrentDirectory());

            //Console.Read();

            new Hook(typeof(File).GetMethod("Exists"), typeof(Antycheat).GetMethod("FileExists"));
            new Hook(typeof(Directory).GetMethod("Exists"), typeof(Antycheat).GetMethod("DirectoryExists"));
            Antycheat.origFileSystemEnumerableIterator = new NativeDetour(typeof(Directory).Assembly.GetTypes().First(x => x.Name.Contains("FileSystemEnumerableIterator"))
               .MakeGenericType(typeof(object)).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
               .First(x => x.GetParameters().Length == 6), typeof(Antycheat).GetMethod("FileSystemEnumerableIterator"),
               new NativeDetourConfig() { SkipILCopy = true }).GenerateTrampoline<Antycheat.FileSystemEnumerableIteratorDelegate>();
            new Hook(typeof(FileStream).GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Antycheat).GetMethod("FileStreamInit"));
            Antycheat.origAppDomainnGetAssemblies = new NativeDetour(typeof(AppDomain).GetMethod("nGetAssemblies", 
                BindingFlags.Instance | BindingFlags.NonPublic), typeof(Antycheat).GetMethod("AppDomainnGetAssemblies"), 
                new NativeDetourConfig() { SkipILCopy = true } ).GenerateTrampoline<Antycheat.AppDomainnGetAssembliesDeleghate>();

            try
            {
                int steamId = (int)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\ActiveProcess", "ActiveUser", new Random().Next());
                HWID.Seed = steamId;
                Log.Danger("Set hwid seed to: " + steamId);
            } 
            catch (Exception ex)
            {
                Log.Danger("Failed to get current steamid, exiting to prevent detection");
                Log.Danger(ex.ToString());
                Log.Danger("Press any key to exit");
                Console.Read();
                Environment.Exit(10);
            }

            Log.Info("Waiting for debugger to attach");
            //while (!Debugger.IsAttached)
            {
                Thread.Sleep(100);
            }
            Log.Info("Debugger attached");
            //Debugger.Break();

            Assembly assembly = Assembly.Load("sectorsedge");

            string[] a = Environment.GetCommandLineArgs();
            string[] args = new string[a.Length - 1];
            Array.Copy(a, 1, args, 0, a.Length - 1);
            Log.Info("args: " + string.Join(" ", args));

            //Log.Info(SignatureManager.GenerateSig("#=zglhlPSH$mnF9ZTdb8WcJdr3OddCUQAd0Aw=="));
            try
            {
                SignatureManager.FindSignatures(assembly);
                
                new Hook(SignatureManager.RequestHelper_POST, typeof(RequestHelper).GetMethod("POST"));
                new Hook(SignatureManager.RequestHelper_GET, typeof(RequestHelper).GetMethod("GET"));
                
                new Hook(SignatureManager.GClass49_vmethod_4, typeof(GClass49).GetMethod("vmethod_4"));

                //new Hook(typeof(SteamFriends).GetMethod("GetPersonaName"), typeof(Steam).GetMethod("GetPersonaName"));

                new Hook(typeof(ManagementBaseObject).GetMethod("GetPropertyValue", BindingFlags.Public | BindingFlags.Instance),
                    typeof(HWID).GetMethod("ManagementBaseObject_GetPropertyValue"));
                new NativeDetour(SignatureManager.Helper1_GetProcAddress, typeof(HWID).GetMethod("GetProcAddress"));
                new Detour(typeof(Environment).GetMethod("get_MachineName", BindingFlags.Public | BindingFlags.Static),
                    typeof(HWID).GetMethod("get_MachineName"));
                new Detour(typeof(Environment).GetMethod("get_UserName", BindingFlags.Public | BindingFlags.Static),
                    typeof(HWID).GetMethod("get_UserName"));
                new Detour(typeof(File).GetMethod("GetCreationTimeUtc", BindingFlags.Public | BindingFlags.Static),
                    typeof(HWID).GetMethod("GetCreationTimeUtc"));
                new Detour(typeof(Directory).GetMethod("GetCreationTimeUtc", BindingFlags.Public | BindingFlags.Static),
                    typeof(HWID).GetMethod("GetCreationTimeUtc"));
                foreach (MethodInfo method in SignatureManager.RegQueryValueEx)
                    HWID.oRegQueryValueEx = new NativeDetour(method, typeof(HWID).GetMethod("RegQueryValueEx"))
                        .GenerateTrampoline<HWID.RegQueryValueExDelegate>();
                new NativeDetour(SignatureManager.DiscordCreate, typeof(HWID).GetMethod("DiscordCreate"));
            
            }
            catch (Exception e)
            {
                Log.Danger(e.ToString());

            }

            MethodInfo mi = assembly.GetType("#=qlP7Rck8fKTTAfxJeTbAdpGzgOJ5BuLGTE8xrRZOLGDs=")
                .GetMethod("#=zD9zupq9kGin4NH9xUv6i3e4=", BindingFlags.NonPublic | BindingFlags.Static);

            AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(assembly.Location);

            //MethodDefinition md = definition.MainModule.GetType("#=qlP7Rck8fKTTAfxJeTbAdpGzgOJ5BuLGTE8xrRZOLGDs=")
            //    .Methods.First(x => x.Name == "#=zE9ylfdY=");

            //FileStream fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "strings.txt"), FileMode.Create);
            //StreamWriter sw = new StreamWriter(fileStream);

            //foreach (var type in definition.MainModule.Types)
            //{
            //    foreach (var m in type.Methods)
            //    {
            //        if (m.HasBody)
            //        {
            //            int id = 0;
            //            foreach (var il in m.Body.Instructions)
            //            {
            //                if (il.OpCode == OpCodes.Ldc_I4)
            //                {
            //                    id = (int)il.Operand;
            //                }
            //                if (il.OpCode == OpCodes.Call)
            //                {
            //                    var mRef = il.Operand as MethodReference;
            //                    if (mRef != null && string.Equals(mRef.FullName, md.FullName, StringComparison.InvariantCultureIgnoreCase))
            //                    {
                                    
            //                        string s = (string)mi.Invoke(null, new object[] { id, true });
            //                        Log.Info(s);
            //                        sw.WriteLine(id.ToString() + ": " + s);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //sw.Close();
            //fileStream.Close();

            //Log.Danger(md.FullName);

            //for(int i = -2001000000; i > -2010000000; i--)
            //{
            //    if (i % 10000 == 0)
            //        Console.Title = i.ToString();
            //    try
            //    {
            //        string x = (string)mi.Invoke(null, new object[] { i, true });
            //        if(x.Length < 200)
            //            Log.Info(x);
            //    } catch (Exception)
            //    {
            //        //Log.Danger(ex.ToString());
            //    }
            //}

            //Log.Danger((string)mi.Invoke(null, new object[] { -2001122674, true }));
            new Hook(mi, typeof(Main).GetMethod("xd"));
            //Console.Read();

            Detour mainDetour = new Detour(assembly.EntryPoint, typeof(Main).GetMethod("MainHook"));
            
            MethodInfo nExecuteAssembly = typeof(AppDomain).GetMethod("nExecuteAssembly", BindingFlags.NonPublic | BindingFlags.Instance);
           
            nExecuteAssembly.Invoke(AppDomain.CurrentDomain, new object[] { assembly, args });

            mainDetour.Dispose(); // rstore it

            for(int i = 0; i < 10; i ++)
                new NativeDetour(typeof(Main).GetMethod("MainLoader" + i), assembly.EntryPoint);

            try
            {
                //MainLoader(args);
                IntPtr xd = GetModuleHandle(Path.GetFileName(Assembly.GetExecutingAssembly().Location));
                if(xd != IntPtr.Zero)
                {
                    IntPtr mainLoader = GetProcAddress(xd, "MainLoader" + args.Length) ;
                    Log.Danger(mainLoader.ToString("X") + " " + args.Length);
                    Console.Read();

                    IntPtr data = Marshal.AllocHGlobal(1024 * 10);
                    int offset = Marshal.SizeOf<IntPtr>() * args.Length;
                    IntPtr lastAddr = IntPtr.Add(data, offset);
                    for(int i = 0; i < args.Length; i++)
                    {
                        byte[] s = Encoding.ASCII.GetBytes(args[i] + "\0");
                        Marshal.Copy(s, 0, lastAddr, s.Length);
                        byte[] addy = BitConverter.GetBytes(lastAddr.ToInt64());
                        Marshal.Copy(addy, 0, new IntPtr(data.ToInt64() + (i * Marshal.SizeOf<IntPtr>())), addy.Length);
                        lastAddr = new IntPtr(lastAddr.ToInt64() + s.Length);
                    }
                    Log.Danger(data.ToInt64().ToString("X"));
                    //Marshal.GetDelegateForFunctionPointer<FakeMainDelegate>(mainLoader)(data);

                    CreateThread(UIntPtr.Zero, 0, mainLoader, data, 0, IntPtr.Zero);


                }
            } 
            catch(Exception ex)
            {
                Log.Danger(ex.ToString());
            }
            //MainLoader(args);
            //origMain(args);

        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private unsafe static extern uint CreateThread(
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

        public delegate void MainDelegate(string[] args);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void MainHook(string[] args)
        {
            Log.Danger("normal Main prevented: " + string.Join(" ", args));
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

    }
}
