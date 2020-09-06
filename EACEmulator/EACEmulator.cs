using MonoMod.RuntimeDetour;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
			Log.Eac("Running from: " + Assembly.GetExecutingAssembly().Location);
			Log.Eac("Running in domain: " + AppDomain.CurrentDomain.FriendlyName);
			Log.Eac("Domain base dir: " + AppDomain.CurrentDomain.BaseDirectory);
			Log.Eac("Working directory: " + Directory.GetCurrentDirectory());

			//Console.Read();

			Assembly assembly = Assembly.Load("sectorsedge");

			string[] a = Environment.GetCommandLineArgs();
			string[] args = new string[a.Length - 1];
			Array.Copy(a, 1, args, 0, a.Length - 1);
			Log.Eac("process args: " + string.Join(" ", args));

			MethodInfo[] eac_funcs = assembly.GetType("#=z5Yqy3S3egHnqI0lLeEnH1ZMw$GJCKrFMNkluyr8=")
					.GetNestedType("#=zJyN_ZSM9809$", BindingFlags.NonPublic).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

			foreach (MethodInfo mi in eac_funcs)
				new NativeDetour(mi, typeof(EACEmulator).GetMethod(mi.GetCustomAttribute<DllImportAttribute>().EntryPoint));

			typeof(AppDomain).GetMethod("nExecuteAssembly", BindingFlags.NonPublic | BindingFlags.Instance)
				.Invoke(AppDomain.CurrentDomain, new object[] { assembly, args });

		}

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AllocConsole();

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IntPtr CreateGameClient(string a1)
		{
            Log.Eac($"CreateGameClient({a1})");
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
			Log.Eac($"GameClient_Initialize({a1}, {(a2 ?? "[null]")}, {(a2 ?? "[null]")}, {a4})");
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
