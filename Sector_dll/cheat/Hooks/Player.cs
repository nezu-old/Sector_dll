using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
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

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);

        public static int aimbot_target_index = -1;

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
                Vec3 local_head = sdk.Player.GetHeadPos(local);

                List<object> players = GameManager.GetPlayers(GameManager.instance.Target);
                if(Control.IsKeyLocked(Keys.NumLock))
                {

                    if ((Control.MouseButtons & (MouseButtons.Left | MouseButtons.Right)) == (MouseButtons.Left | MouseButtons.Right))
                    {
                        if (aimbot_target_index == -1)
                        {
                            double last_diff = double.MaxValue;
                            TeamType my_team = sdk.Player.GetTeam(local);
                            for (int i = 0; i < players.Count; i++)
                            {
                                object player = players[i];

                                if (sdk.Player.GetTeam(player) == my_team)
                                    continue;

                                BoneManager.SetupBones(player, i);

                                WorldSpaceBone head = BoneManager.HeadBones[i];

                                Vec3 aim_vec = local_head - head.head;// sdk.Helper.Lerp(head.head, head.tail, 0.5); //middle of the head bone;

                                Vec2 new_a = sdk.Helper.VectorAngles(aim_vec);

                                Vec2 my_a = new Vec2(sdk.Player.GetYaw(local), sdk.Player.GetPitch(local));

                                Vec2 diff_a = new_a - my_a;
                                while (diff_a.x > 3.141591653589793) diff_a.x += -3.141591653589793 * 2;
                                while (diff_a.x < -3.141591653589793) diff_a.x += 3.141591653589793 * 2;

                                double diff = diff_a.Len();
                                if (diff < last_diff)
                                {
                                    last_diff = diff;
                                    aimbot_target_index = i;
                                }

                            }
                            Log.Debug("target!");
                        }

                        if (aimbot_target_index != -1)
                        {

                            WorldSpaceBone head = BoneManager.HeadBones[aimbot_target_index];

                            Vec3 aim_vec = local_head - head.head;// sdk.Helper.Lerp(head.head, head.tail, 0.5); //middle of the head bone;

                            Vec2 new_a = sdk.Helper.VectorAngles(aim_vec);

                            Vec2 my_a = new Vec2(sdk.Player.GetYaw(local), sdk.Player.GetPitch(local));

                            Vec2 diff_a = new_a - my_a;
                            while (diff_a.x > 3.141591653589793) diff_a.x += -3.141591653589793 * 2;
                            while (diff_a.x < -3.141591653589793) diff_a.x += 3.141591653589793 * 2;

                            diff_a /= 20.0;

                            Drawing.DrawText(diff_a.ToString(), 130, 100);

                            sdk.Player.SetYaw(self, my_a.x + diff_a.x);
                            sdk.Player.SetPitch(self, my_a.y + diff_a.y);
                        }
                    }
                    else 
                        aimbot_target_index = -1;

                }

                if ((GetAsyncKeyState(Keys.O) & 0x1) == 0x1)
                {
                    object pp = sdk.Player.New(gm, 2);
                    sdk.Player.SetTeam(pp, TeamType.Helix);
                    sdk.Player.SetOrigin(pp, new Vec3(165.0, 35.0, 38.0));
                    //pp.GetType().GetMethod("#=zPQmkMUfB4gF$").Invoke(pp, new object[] { Enum.ToObject(pp.GetType().GetMethod("#=zPQmkMUfB4gF$").GetParameters()[0].ParameterType, 7) });
                    SignatureManager.PlayerBase_health.SetValue(pp, 100.0);
                    SignatureManager.PlayerBase_name.SetValue(pp, "Player");

                    object all_pp = SignatureManager.GClass49_player_list.GetValue(gm);
                    SignatureManager.GClass49_player_list.FieldType.GetMethod("Add").Invoke(all_pp, new[] { pp });

                    //SignatureManager.GameManager.GetField("#=z18zI4Xnk8_$T").SetValue(gm, Enum.ToObject(SignatureManager.GameManager.GetField("#=z18zI4Xnk8_$T").FieldType, Config.settings.debug3));

                }
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
