using Mono.Cecil;
using MonoMod.RuntimeDetour;
using Sector_dll.cheat.Hooks;
using Sector_dll.util;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Sector_dll.cheat
{

    public class Main
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string Xd(Func<int, bool, string> orig, int i, bool b)//Func<IntPtr, string, IntPtr> orig, 
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

        public unsafe static void Entry()
        {
            //AllocConsole();
            Log.enabled = true;
            Log.Prefix = "[nezu.cc]";

            Log.Info("Entry point called");
            //Log.Info("Running from: " + (Assembly.GetExecutingAssembly().Location.Trim().Length == 0 ? "[Memory]" : Assembly.GetExecutingAssembly().Location));
            //Log.Info("Running in domain: " + AppDomain.CurrentDomain.FriendlyName);
            //Log.Info("Domain base dir: " + AppDomain.CurrentDomain.BaseDirectory);
            //Log.Info("Working directory: " + Directory.GetCurrentDirectory());
            ReversibleRenamer.inst = new ReversibleRenamer("fuckverc");

            //new Hook(typeof(File).GetMethod("Exists"), typeof(Antycheat).GetMethod("FileExists"));
            //new Hook(typeof(Directory).GetMethod("Exists"), typeof(Antycheat).GetMethod("DirectoryExists"));
            //Antycheat.origFileSystemEnumerableIterator = new NativeDetour(typeof(Directory).Assembly.GetTypes()
            //    .First(x => x.Name.Contains("FileSystemEnumerableIterator")).MakeGenericType(typeof(object))
            //    .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.GetParameters().Length == 6), 
            //    typeof(Antycheat).GetMethod("FileSystemEnumerableIterator"), new NativeDetourConfig() { SkipILCopy = true })
            //    .GenerateTrampoline<Antycheat.FileSystemEnumerableIteratorDelegate>();
            ////new Hook(typeof(FileStream).GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Antycheat).GetMethod("FileStreamInit"));
            //Antycheat.origAppDomainnGetAssemblies = new NativeDetour(typeof(AppDomain).GetMethod("nGetAssemblies", 
            //    BindingFlags.Instance | BindingFlags.NonPublic), typeof(Antycheat).GetMethod("AppDomainnGetAssemblies"), 
            //    new NativeDetourConfig() { SkipILCopy = true } ).GenerateTrampoline<Antycheat.AppDomainnGetAssembliesDeleghate>();

            //try
            //{
            //    int steamId = (int)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\ActiveProcess", "ActiveUser", new Random().Next());
            //    HWID.Seed = steamId;
            //    Log.Debug("Set hwid seed to: " + steamId);
            //} 
            //catch (Exception ex)
            //{
            //    Log.Danger("Failed to get current steamid, exiting to prevent detection");
            //    Log.Danger(ex.ToString());
            //    Log.Danger("Press any key to exit");
            //    Console.Read();
            //    Environment.Exit(10);
            //}

            //while (!Debugger.IsAttached) Thread.Sleep(100);

            Assembly assembly = Assembly.GetEntryAssembly();

            //Log.Debug(SignatureManager.GenerateSig("#=zaRmD6HfM_mntgBqTQB3dfALAvnZv")); Console.Read();

            try
            {
                if (!SignatureManager.FindSignatures(assembly))
                    throw new Exception("Failed to resolve all types/methods/fields");

                //new Hook(SignatureManager.RequestHelper_POST, typeof(RequestHelper).GetMethod("POST"));
                //new Hook(SignatureManager.RequestHelper_GET, typeof(RequestHelper).GetMethod("GET"));

                //new Thread(() =>
                //{
                //    try
                //    {
                //        Type type = assembly.GetType("#=zjIQApYtv96HwCbHWyPQ5EKqErtHs", true);
                //        Log.Debug(type);
                //        //FieldInfo fieldInfo = type.GetField("#=zfxpo$Rw=", BindingFlags.Public | BindingFlags.Static);
                //        while (true)
                //        {
                //            //object val = fieldInfo.GetValue(null);
                //            //Log.Debug(val ?? "[null]");
                //            //Thread.Sleep(1000);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Log.Debug(ex);
                //    }
                //}).Start();

                Log.Debug("All types resolved");

                new Hook(SignatureManager.SwapBuffersWrapper, new Action<Action<object, object>, object, object>((orig, self, a1) => GL.SwapBuffers(orig, self, a1)).Method, new object());
                new Hook(SignatureManager.GClass49_Base_Base_Draw, new Action<Action<object, object>, object, object>((orig, self, a1) => GClass49.vmethod_4(orig, self, a1)).Method, new object());
                new Hook(SignatureManager.LocalPlayer_Update, new Action<Action<object, object>, object, object>((orig, self, a1) => Player.Update(orig, self, a1)).Method, new object());

                //new Hook(SignatureManager.PlayerBase_RecoilMod, typeof(Player).GetMethod("RecoilMod"));

                //new Hook(SignatureManager.Helper_CurrentBloom, typeof(Helper).GetMethod("CurrentBloom"));

            }
            catch (Exception e)
            {
                Log.Danger(e.ToString());
                Log.Danger("Press any key to exit");
                Console.ReadKey(true);
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

            //typeof(AppDomain).GetMethod("nExecuteAssembly", BindingFlags.NonPublic | BindingFlags.Instance)
            //    .Invoke(AppDomain.CurrentDomain, new object[] { assembly, args });

            //Console.ReadLine();

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

        MethodInfo GetMethodInfo(Action a) => a.Method;

        //[DllImport("kernel32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool AllocConsole();

    }
}
