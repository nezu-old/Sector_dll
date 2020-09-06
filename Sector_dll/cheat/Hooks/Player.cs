using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                if (Config.settings.debug6 > 0) Log.Info("Player");
                BoneManager.InvalidateBones();

                object gm = GameManager.instance.Target;
                object local = GameManager.GetLocalPLayer(gm);
                if (local == null)
                    return;
                //Vec3 local_head = sdk.Player.GetHeadPos(local);

                List<object> players = GameManager.GetPlayers(GameManager.instance.Target);
                //for (int i = 0; i < players.Count; i++)
                //{
                //    object player = players[i];
                //    BoneManager.SetupBones(player, i);

                //    WorldSpaceBone head = BoneManager.HeadBones[i];

                //    Vec3 aim_vec = local_head - head.head;// sdk.Helper.Lerp(head.head, head.tail, 0.5); //middle of the head bone;

                //    Vec2 ang = sdk.Helper.VectorAngles(aim_vec);

                //    sdk.Player.SetYaw(self, ang.x);
                //    sdk.Player.SetPitch(self, ang.y);


                //    if (Config.settings.debug6 > 0) Log.Info("XD: " + sdk.Player.GetName(player));
                //}

                //if (players.Count < 1)
                //{
                //    object pp = sdk.Player.New(gm, 10);
                //    sdk.Player.SetTeam(pp, TeamType.Helix);
                //    sdk.Player.SetOrigin(pp, new Vec3(165.0, 35.0, 38.0));
                //    //pp.GetType().GetMethod("#=zPQmkMUfB4gF$").Invoke(pp, new object[] { Enum.ToObject(pp.GetType().GetMethod("#=zPQmkMUfB4gF$").GetParameters()[0].ParameterType, 7) });
                //    SignatureManager.PlayerBase_health.SetValue(pp, 100.0);
                //    SignatureManager.PlayerBase_name.SetValue(pp, "BOT");



                //    object all_pp = SignatureManager.GClass49_player_list.GetValue(gm);
                //    SignatureManager.GClass49_player_list.FieldType.GetMethod("Add").Invoke(all_pp, new[] { pp });

                //    if (Config.settings.debug6 > 0) Log.Info("BOT: " + pp);
                //    //SignatureManager.GameManager.GetField("#=z18zI4Xnk8_$T").SetValue(gm, Enum.ToObject(SignatureManager.GameManager.GetField("#=z18zI4Xnk8_$T").FieldType, Config.settings.debug3));

                //}
                //else
                //{
                //    //object pp = players[0];

                //    //pp.GetType().GetMethod("#=zC$PwqoXEojeRikU4rg==").Invoke(pp, new object[] { Config.settings.debug5 > 0 });

                //    //sdk.Player.SetYaw(pp, sdk.Player.GetYaw(pp) + 0.05);
                //    //sdk.Player.SetPitch(pp, 0.2);
                //    //d.DrawText(sdk.Player.GetLookAtVector(pp).ToString(), 100, 250);

                //    //Vec3 xd = Player.GetOrigin(pp);
                //    //xd.x += 0.001;
                //    //Player.SetOrigin(pp, xd);
                //}
            }
        }

    }
}
