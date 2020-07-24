using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    class Player
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static double RecoilMod(Func<object, double> orig, object self)
        {
            double mod = orig(self);
            mod *= 0;
            return mod;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Update(Action<object, object> orig, object self, object a1)
        {
            orig(self, a1);

            if (GameManager.instance.IsAlive)
            {
                List<object> players = GameManager.GetPlayers(GameManager.instance.Target);
                foreach(object player in players)
                {
                    GameManager.SetupBones(GameManager.instance.Target, player);
                }
            }

            if (Config.settings.debug6 > 0) Log.Info("Player");
            //sdk.Player.SetPitch(self, 0);
            //sdk.Player.SetYaw(self, 0);
        }

    }
}
