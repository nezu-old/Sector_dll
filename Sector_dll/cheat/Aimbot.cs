using Sector_dll.cheat;
using Sector_dll.sdk;
using sectorsedge.sdk;
using sectorsedge.util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Sector_dll.cheat.Config;

namespace sectorsedge.cheat
{
    class Aimbot
    {

        public static int lockedTarget = -1;

        public static int currentTargetID = -1;

        public static void UpdateTarget(List<object> players, object local)
        {
            //if (currentTargetID >= 0 && !settings.aimbot_auto_switch_target)
            //    return;

            int best_id = -1;
            Vec3 local_head = Player.GetHeadPos(local);
            Vec3 shoot_end = local_head + CollisionHelper.GetShootVectorBase(local);

            double last_dist = double.MaxValue;
            TeamType my_team = Player.GetTeam(local);

            int c = Math.Min(lockedTarget < 0 ? players.Count : lockedTarget + 1, players.Count);
            for (int i = lockedTarget < 0 ? 0 : lockedTarget; i < c; i++)
            {
                object player = players[i];

                if (Player.GetHealth(player) <= 0)
                    continue;

                if (Player.GetTeam(player) == my_team)
                    continue;

                BoneManager.SetupBones(player, i);
                BoneManager.VisibilityCheck(local_head, i);

                if ((settings.aimbot_penetration == AimbotPenetration.IfAnnyVisible || settings.aimbot_penetration == AimbotPenetration.OnlyVisible) && (GameManager.PlayerFlags[i] & PlayerFlags.Visible) == 0)
                    continue;

                double dist = Math.Min(Math3D.DistanceToSegment(BoneManager.HeadBones[i].tail, local_head, shoot_end), Math3D.DistanceToSegment(Player.GetOrigin(player), local_head, shoot_end));
                if (dist < last_dist)
                {
                    last_dist = dist;
                    best_id = i;// Player.GetID(player);
                }

            }
            if (settings.aimbot_fov > 0 && last_dist > settings.aimbot_fov)
            {
                best_id = -1; //closest one is out of fov
            }

            currentTargetID = best_id;

            if (!settings.aimbot_auto_switch_target && lockedTarget < 0)
                lockedTarget = best_id;
        }

        public static void Run(object local)
        {
            object gm = GameManager.instance.Target;
            List<object> players = GameManager.GetPlayerList(gm);
            if (settings.aimbot_mode == AimbotMode.Off || Player.GetHealth(local) <= 0 || (settings.aimbot_mode == AimbotMode.OnShoot && !InputManager.IsKeyPressed(InputManager.Shoot)))
            {
                currentTargetID = -1;
                lockedTarget = -1;
                return;
            }
            UpdateTarget(players, local);

            if (currentTargetID < 0 || currentTargetID >= players.Count)
                return;

            //object target = players[currentTargetID];

            Vec3 local_head = Player.GetHeadPos(local);

            WorldSpaceBone bone = null;
            if (settings.aimbot_penetration == AimbotPenetration.OnlyVisible)
            {
                if((BoneManager.HeadBones[currentTargetID].flags & WorldSpaceBone.Flags.Visible) != 0)
                    bone = BoneManager.HeadBones[currentTargetID];
                else {
                    foreach (var b in BoneManager.BoneCache[currentTargetID])
                    {
                        if((b.flags & WorldSpaceBone.Flags.Visible) != 0)
                        {
                            bone = b;
                            break; //first visible //TODO: pick best damage from all visble
                        }
                    }
                    if (bone == null)
                        throw new Exception("no bone visble, should never hit");
                }
            }
            else
                bone = BoneManager.HeadBones[currentTargetID];

            bone.flags |= WorldSpaceBone.Flags.Aimboting;

            Vec3 aim_vec = local_head - Helper.Lerp(bone.head, bone.tail, 0.5);

            Vec2 new_a = Helper.VectorAngles(aim_vec);
            Vec2 my_a = new Vec2(Player.GetYaw(local), Player.GetPitch(local));

            Vec2 diff_a = new_a - my_a;
            while (diff_a.x > 3.141591653589793)  diff_a.x += -3.141591653589793 * 2;
            while (diff_a.x < -3.141591653589793) diff_a.x +=  3.141591653589793 * 2;

            diff_a /= settings.aimbot_smooth;

            my_a += diff_a;

            Player.SetYaw  (local, my_a.x);
            Player.SetPitch(local, my_a.y);

            if(settings.aimbot_auto_shoot)
            {
                double dist = Math3D.LineToLineDistance(local_head, local_head + CollisionHelper.GetShootVectorBase(local), bone.head, bone.tail);
                if(dist <= bone.radius)
                {
                    InputManager.SetKeyPressed(InputManager.Shoot, true);
                    Task.Run(async delegate
                    {
                        await Task.Delay(15);
                        InputManager.SetKeyPressed(InputManager.Shoot, false);
                    });
                }
            }

        }

    }
}
