using MonoMod.RuntimeDetour;
using RGiesecke.DllExport;
using Sector_dll.cheat.Hooks;
using Sector_dll.util;
using Steamworks;
using System;
using System.CodeDom;
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
            if (s.Contains("OpenGL3 init"))
            {
                Log.Info("I: " + i + " S: " + s);
                StackTrace t = new StackTrace();
                Log.Info(t.ToString());
            }
            return s;
        }

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



            //Log.Info("Waiting for debugger to attach");
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(100);
            //}
            //Log.Info("Debugger attached");
            //Debugger.Break();

            Assembly assembly = Assembly.Load("sectorsedge");

            try
            {
                //Directory.GetFiles(@"C:\Users\admin\Documents\GitHub\Sector_dll", "*.*");
            } catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            string[] a = Environment.GetCommandLineArgs();
            string[] args = new string[a.Length - 1];
            Array.Copy(a, 1, args, 0, a.Length - 1);
            Log.Info(string.Join(" ", args));

            //Log.Info(SignatureManager.GenerateSig("#=zglhlPSH$mnF9ZTdb8WcJdr3OddCUQAd0Aw=="));
            try
            {
                SignatureManager.FindSignatures(assembly);
                
                new Hook(SignatureManager.RequestHelper_POST, typeof(RequestHelper).GetMethod("POST"));
                new Hook(SignatureManager.RequestHelper_GET, typeof(RequestHelper).GetMethod("GET"));
                
                new Hook(SignatureManager.GClass49_vmethod_4, typeof(GClass49).GetMethod("vmethod_4"));

                new Hook(typeof(SteamFriends).GetMethod("GetPersonaName"), typeof(Steam).GetMethod("GetPersonaName"));
                new Hook(typeof(ManagementBaseObject).GetMethod("GetPropertyValue", BindingFlags.Public | BindingFlags.Instance),
                    typeof(HWID).GetMethod("ManagementBaseObject_GetPropertyValue"));
                new NativeDetour(SignatureManager.Helper1_GetProcAddress, typeof(HWID).GetMethod("GetProcAddress"));
            
            }
            catch (Exception e)
            {
                Log.Danger(e.ToString());
            }

            MethodInfo mi = assembly.GetType("#=qlP7Rck8fKTTAfxJeTbAdpGzgOJ5BuLGTE8xrRZOLGDs=")
                .GetMethod("#=zD9zupq9kGin4NH9xUv6i3e4=", BindingFlags.NonPublic | BindingFlags.Static);
            //Log.Info(mi.ToString());
            new Hook(mi, typeof(Main).GetMethod("xd"));

            //aassembly.GetType("#=zQxVHBMHIj9grktU86Yg6iqY=").GetMethod("#=zeQBGTj4pRBby").Invoke(null, new object[] { });

            MethodInfo nExecuteAssembly = typeof(AppDomain).GetMethod("nExecuteAssembly", BindingFlags.NonPublic | BindingFlags.Instance);
            //assembly.GetType("#=zEjAcxtmYhCCR4kBIPIMqc1w=").GetMethod("#=zrFEMhxKgaxhz").Invoke(null, new object[] { });

            nExecuteAssembly.Invoke(AppDomain.CurrentDomain, new object[] { assembly, args });

        }

        private static Assembly Domain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Log.Danger(args.Name);
            
            return null;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

    }
}
