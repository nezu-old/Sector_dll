using Sector_dll.util;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    class Steam
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetPersonaName(Func<string> orig)
        {
            HWID.Seed = (int)(SteamUser.GetSteamID().m_SteamID - 76561197960265728);
            Log.Info("Set HWID seed to: " + HWID.Seed);
            return orig();
        }

    }
}
