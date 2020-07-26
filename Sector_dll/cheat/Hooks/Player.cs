using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                object gm = GameManager.instance.Target;
                object local = GameManager.GetLocalPLayer(gm);
                if (local == null)
                    return;
                Vec3 local_head = sdk.Player.GetHeadPos(local);

                List<object> players = GameManager.GetPlayers(GameManager.instance.Target);
                for (int i = 0; i < players.Count; i++)
                {
                    object player = players[i];
                    BoneManager.SetupBones(player, i);

                    WorldSpaceBone head = BoneManager.HeadBones[i];

                    Vec3 aim_vec = local_head - head.head;// sdk.Helper.Lerp(head.head, head.tail, 0.5); //middle of the head bone;

                    Vec2 ang = sdk.Helper.VectorAngles(aim_vec);

                    sdk.Player.SetYaw(self, ang.x);
                    sdk.Player.SetPitch(self, ang.y);


                    if (Config.settings.debug6 > 0) Log.Info("XD: " + sdk.Player.GetName(player));
                }
            }

            if (Config.settings.debug6 > 0) Log.Info("Player");
        }

    }
}
