using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace EACEmulator
{
    public class Log
    {

		public static void Eac(string msg)
        {
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine("[EACEmulator] " + msg);
			Console.ResetColor();
		}

	}

    public class EACEmulator
    {
		
		public static void Main() {

			AllocConsole();
			Log.Eac("emulator starting");
			Log.Eac("Running from: " + (Assembly.GetExecutingAssembly().Location.Trim().Length == 0 ? "[Memory]" : Assembly.GetExecutingAssembly().Location));
			Log.Eac("Running in domain: " + AppDomain.CurrentDomain.FriendlyName);
			Log.Eac("Domain base dir: " + AppDomain.CurrentDomain.BaseDirectory);
			Log.Eac("Working directory: " + Directory.GetCurrentDirectory());

			Assembly assembly = Assembly.Load("sectorsedge");

			string[] a = Environment.GetCommandLineArgs();
			string[] args = new string[a.Length - 1];
			Array.Copy(a, 1, args, 0, a.Length - 1);
			Log.Eac("process args: " + string.Join(" ", args));

			List<Type> eac_l = assembly.GetTypes()
				.Select(type => type.GetNestedTypes(BindingFlags.NonPublic).ToList())
				.SelectMany(x => x)
				.Distinct()
				.Where(type => type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
					.Any(method =>
						method.GetCustomAttribute<DllImportAttribute>() != null &&
						method.GetCustomAttribute<DllImportAttribute>().Value == "easyanticheat"
					)
				).ToList();
			if(eac_l == null && eac_l.Count == 2)
            {
				Log.Eac("Failed to find eac classes!!\n\nPress any key to exit");
				Console.ReadKey(true);
				Environment.Exit(1);
				return;
			}

            foreach (var eac in eac_l)
            {
				Log.Eac("Found eac class as: " + eac.ToString());
				MethodInfo[] eac_funcs = eac.GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
				//int expected = 16;
				//if (eac_funcs.Length != expected)
				//{
				//	Log.Eac($"Number of eac functions does not match. Got {eac_funcs.Length} expected {expected}!!\n\nPress any key to exit");
				//	Console.ReadKey(true);
				//	Environment.Exit(1);
				//	return;
				//}

				foreach (MethodInfo mi in eac_funcs)
				{
					MethodInfo hook_mi = typeof(EACEmulator).GetMethod(mi.GetCustomAttribute<DllImportAttribute>().EntryPoint);
					NativeDetour detour = new NativeDetour(mi, hook_mi);
					if (detour.IsApplied && detour.IsValid)
					{
						Log.Eac("Hooked " + mi.Name + " -> " + hook_mi.Name);
					}
					else
					{
						Log.Eac("Failed to hoook " + mi.Name + "\n\nPress any key to exit");
						Console.ReadKey(true);
						Environment.Exit(1);
						return;
					}
				}
			}

            //        new Thread(() =>
            //        {
            //if(MessageBox.Show("Inject?", "[nezu.cc]", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //            {
            //            }
            //        }).Start();

            //new Thread(() =>
            //{
            byte[] bytes = File.ReadAllBytes(@"C:\Users\admin\Desktop\Sector_dll\Release\Sector_dll.dll");
            Assembly.Load(bytes).GetType("EAC").GetMethod("Main").Invoke(null, null);
            ////}).Start();

            Console.ReadKey(true);

            //if (MessageBox.Show("Load Game?", "[nezu.cc]", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            typeof(AppDomain).GetMethod("nExecuteAssembly", BindingFlags.NonPublic | BindingFlags.Instance)
					.Invoke(AppDomain.CurrentDomain, new object[] { assembly, args });

		}

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AllocConsole();

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IntPtr CreateGameClient(string a1)
		{
            Log.Eac($"CreateGameClient({a1 ?? "[null]"})");
			return (IntPtr)1337;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_Destroy(IntPtr a1)
		{
			Log.Eac($"GameClient_Destroy({a1})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_Initialize(IntPtr a1, object a2, object a3, IntPtr a4)
		{
			Log.Eac($"GameClient_Initialize({a1}, {a2 ?? "[null]"}, {a3 ?? "[null]"}, {a4})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClientP2P_ResetState(IntPtr a1)
        {
			Log.Eac($"GameClientP2P_ResetState({a1})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool GameClientP2P_RegisterPeer(IntPtr a1, IntPtr a2, string a3, string a4, string a5)
		{
			Log.Eac($"GameClientP2P_RegisterPeer({a1}, {a2}, {a3 ?? "[null]"}, {a4 ?? "[null]"}, {a5 ?? "[null]"})");
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClientP2P_UnregisterPeer(IntPtr a1, IntPtr a2)
		{
			Log.Eac($"GameClientP2P_UnregisterPeer({a1}, {a2})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool GameClientP2P_BeginSession(IntPtr a1, uint a2, string a3, string a4, byte[] a5, uint a6)
		{
			Log.Eac($"GameClientP2P_BeginSession({a1}, {a2}, {a3 ?? "[null]"}, {a4 ?? "[null]"}, {a5}, {a6})");
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClientP2P_EndSession(IntPtr a1)
		{
			Log.Eac($"GameClientP2P_EndSession({a1})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IntPtr GameClientP2P_PollForMessageToPeer(IntPtr a1, IntPtr a2, out IntPtr a3, out uint a4)
		{
			Log.Eac($"GameClientP2P_PollForMessageToPeer({a1}, {a2})");
			a3 = IntPtr.Zero;
			a4 = 0;
			return IntPtr.Zero;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClientP2P_SetMaxAllowedMessageLength(IntPtr a1, uint a2)
		{
			Log.Eac($"GameClientP2P_SetMaxAllowedMessageLength({a1}, {a2})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClientP2P_ReceiveMessageFromPeer(IntPtr a1, IntPtr a2, byte[] a3, uint a4)
		{
			Log.Eac($"GameClientP2P_ReceiveMessageFromPeer({a1}, {a2}, {a3}, {a4})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClientP2P_PollStatus(IntPtr a1, object a2, object a3, IntPtr a4)
		{
			Log.Eac($"GameClientP2P_PollStatus({a1}, {a2 ?? "[null]"}, {a3 ?? "[null]"}, {a4})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClientP2P_InitLocalization(IntPtr a1, string a2, string a3)
		{
			Log.Eac($"GameClientP2P_InitLocalization({a1}, {a2 ?? "[null]"}, {a3 ?? "[null]"})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IntPtr GameClientP2P_Cerberus(IntPtr a1)
		{
			Log.Eac($"GameClientP2P_Cerberus({a1})");
			return IntPtr.Zero;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClientP2P_SetLogCallback(IntPtr a1, object a2, object a3)
		{
			Log.Eac($"GameClientP2P_SetLogCallback({a1}, {a2 ?? "[null]"}, {a3 ?? "[null]"})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClientP2P_UpdatePlatformUserAuthTicket(IntPtr a1, string a2, byte[] a3, uint a4)
		{
			Log.Eac($"GameClientP2P_UpdatePlatformUserAuthTicket({a1}, {a2 ?? "[null]"}, {a3}, {a4})");
		}

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void GameClient_ConnectionReset(IntPtr a1)
        {
            Log.Eac($"GameClient_ConnectionReset({a1})");

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool GameClient_PopNetworkMessage(IntPtr a1, ref IntPtr a2, out uint a3)
        {
            //Log.Eac($"GameClient_PopNetworkMessage({a1})");
            a2 = IntPtr.Zero;
            a3 = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void GameClient_SetMaxAllowedMessageLength(IntPtr a1, uint len)
        {
            Log.Eac($"GameClient_SetMaxAllowedMessageLength({a1}, {len})");

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void GameClient_PushNetworkMessage(IntPtr a1, byte[] msg, uint a3)
        {
            Log.Eac($"GameClient_PushNetworkMessage({a1}, {msg.Length}, {a3})");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void GameClient_SetPlatformUserAuthTicket(IntPtr a1, string a2, byte[] a3, uint a4)
        {
            Log.Eac($"GameClient_SetPlatformUserAuthTicket({a1}, {a2}, {a3.Length}, {a4})");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void GameClient_PollStatus(IntPtr a1, object a2, object a3, IntPtr a4)
        {
            //Log.Eac($"GameClient_PollStatus({a1}, {(a2 ?? "[null]")}, {(a3 ?? "[null]")}, {a4})");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void GameClient_ValidateServerHost(IntPtr a1, uint a2, IntPtr a3, uint a4)
        {
            Log.Eac($"GameClient_ValidateServerHost({a1}, {a2}, {a3}, {a4})");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IntPtr GameClient_NetProtect(IntPtr a1)
        {
            Log.Eac($"GameClient_NetProtect({a1})");
            return IntPtr.Zero;
        }

    }
}
