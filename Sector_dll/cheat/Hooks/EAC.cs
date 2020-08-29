using Sector_dll.util;
using System;
using System.Runtime.CompilerServices;

namespace Sector_dll.cheat.Hooks
{
    class EAC
    {

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IntPtr CreateGameClient(string a1)
		{
			Log.Debug($"CreateGameClient({a1})");
			return (IntPtr)6969;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_Destroy(IntPtr a1)
		{
			Log.Debug($"GameClient_Destroy({a1})");
		}
		
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_Initialize(IntPtr a1, object a2, object a3, IntPtr a4)
		{
			Log.Debug($"GameClient_Initialize({a1}, {(a2 ?? "[null]")}, {(a2 ?? "[null]")}, {a4})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_ConnectionReset(IntPtr a1)
		{
			Log.Debug($"GameClient_ConnectionReset({a1})");

		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool GameClient_PopNetworkMessage(IntPtr a1, ref IntPtr a2, out uint a3)
		{
			//Log.Debug($"GameClient_PopNetworkMessage({a1})");
			a2 = IntPtr.Zero;
			a3 = 0;
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_SetMaxAllowedMessageLength(IntPtr a1, uint len)
		{
			Log.Debug($"GameClient_SetMaxAllowedMessageLength({a1}, {len})");

		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_PushNetworkMessage(IntPtr a1, byte[] msg, uint a3)
		{
			Log.Debug($"GameClient_PushNetworkMessage({a1}, {msg.Length}, {a3})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_SetPlatformUserAuthTicket(IntPtr a1, string a2, byte[] a3, uint a4)
		{
			Log.Debug($"GameClient_SetPlatformUserAuthTicket({a1}, {a2}, {a3.Length}, {a4})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_PollStatus(IntPtr a1, object a2, object a3, IntPtr a4)
		{
			//Log.Debug($"GameClient_PollStatus({a1}, {(a2 ?? "[null]")}, {(a3 ?? "[null]")}, {a4})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GameClient_ValidateServerHost(IntPtr a1, uint a2, IntPtr a3, uint a4)
		{
			Log.Debug($"GameClient_ValidateServerHost({a1}, {a2}, {a3}, {a4})");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IntPtr GameClient_NetProtect(IntPtr a1)
		{
			Log.Debug($"GameClient_NetProtect({a1})");
			return IntPtr.Zero;	
		}

    }
}
