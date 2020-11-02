using Sector_dll.sdk;
using Sector_dll.util;
using sectorsedge.cheat.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sector_dll.cheat.Drawing;
using static Sector_dll.cheat.Config;
using sectorsedge.sdk;
using sectorsedge.util;
using sectorsedge.cheat;

namespace Sector_dll.cheat
{
    class ESP
    {

        internal static void DrawPlayerEsp()
        {
            object gm = GameManager.instance.Target;

            object local = GameManager.GetLocalPLayer(gm);
            object current = GameManager.GetCurrentPLayer(gm);
            if (settings.esp_mode == EspModes.Off ||
                (settings.esp_mode == EspModes.OnDeath && (local != null && Player.GetHealth(local) > 0)))
                return;

            byte local_team = local != null ? (byte)Player.GetTeam(local) : (byte)0xFF;

            if (settings.debug_esp)
            {
                DrawText(GameManager.ScreenResolution.ToString(), 100, 100);
                GameManager.W2s(new Vec3(0, 0, 0), out Vec2 test);
                {
                    DrawText(test.ToString(), 100, 115);
                    DrawRect((int)test.x - 5, (int)test.y - 5, 10, 10, 2, Color.red);
                }
            }

            Vec3 current_head = Player.GetHeadPos(current);

            List<object> players = GameManager.GetPlayers(gm);
            for (int i = 0; i < players.Count; i++)
            {

                object player = players[i];

                if (player == current)
                    continue;

                byte team = (byte)Player.GetTeam(player);

                if (settings.esp_target != EspTarget.All && (
                    (settings.esp_target == EspTarget.Enemy && team == local_team) ||
                    (settings.esp_target == EspTarget.Team && team != local_team)))
                    continue;

                double hp = Player.GetHealth(player);
                if (hp > 0.0)
                {
                    Vec3 origin = Player.GetOrigin(player);
                    Vec3 origin_plus_1 = origin + new Vec3(0, 1, 0);

                    if (GameManager.W2s(origin, out Vec2 origin2d) && GameManager.W2s(origin_plus_1, out Vec2 origin_plus_12d))
                    {
                        double dist_scale = origin2d.DistTo(origin_plus_12d);
                        Color color = GameManager.GetPlayerColor(gm, player, false);

                        BoneManager.SetupBones(player, i);
                        BoneManager.VisibilityCheck(current_head, i);

                        if (settings.esp_vis_mode == EspVisCheck.OnlyVisible && (GameManager.PlayerFlags[i] & PlayerFlags.Visible) == 0)
                            continue;

                        List<WorldSpaceBone> bones = BoneManager.BoneCache[i];

                        Vec2 bb_min = null;
                        Vec2 bb_max = null;
                        if(settings.esp_box == EspBoxMode.Precise || settings.esp_skeleton)
                        {
                            for (int j = 0; j < bones.Count(); j++)
                            {
                                WorldSpaceBone bone = bones[j];
                                Vec2 head_b = null;
                                if ((!settings.esp_skeleton || GameManager.W2s(bone.head, out head_b)) &&
                                    GameManager.W2s(bone.tail, out Vec2 tail_b))
                                {
                                    double r = dist_scale * bone.radius * 2;
                                    double min_x, min_y, max_x, max_y;
                                    if (settings.esp_skeleton)
                                    {
                                        min_x = Math.Min(tail_b.x, head_b.x) - r;
                                        min_y = Math.Min(tail_b.y, head_b.y) - r;
                                        max_x = Math.Max(tail_b.x, head_b.x) + r;
                                        max_y = Math.Max(tail_b.y, head_b.y) + r;
                                    } 
                                    else
                                    {
                                        min_x = tail_b.x - r;
                                        min_y = tail_b.y - r;
                                        max_x = tail_b.x + r;
                                        max_y = tail_b.y + r;
                                    }


                                    if (bb_min == null)
                                    {
                                        bb_min = new Vec2(min_x, min_y);
                                        bb_max = new Vec2(max_x, max_y);
                                    }
                                    if (min_x < bb_min.x) bb_min.x = min_x;
                                    if (min_y < bb_min.y) bb_min.y = min_y;
                                    if (max_x > bb_max.x) bb_max.x = max_x;
                                    if (max_y > bb_max.y) bb_max.y = max_y;

                                    if (settings.esp_skeleton)
                                    {
                                        DrawLine((int)head_b.x, (int)head_b.y, (int)tail_b.x, (int)tail_b.y, 1, (bone.flags & WorldSpaceBone.Flags.Aimboting) != 0 ? Color.green : (bone.flags & WorldSpaceBone.Flags.Visible) != 0 ? Color.white : color);
                                        //DrawText(bone.name, (int)head_b.x, (int)head_b.y, Color.white, TextAlign.CENTER);
                                    }
                                }
                            }
                        }
                        if(settings.esp_box == EspBoxMode.Standard)
                        {
                            WorldSpaceBone bone = BoneManager.HeadBones[i];
                            if(GameManager.W2s(bone.tail, out Vec2 head2d))
                            {
                                double r = dist_scale * bone.radius * 2;
                                double hh = origin2d.y - head2d.y;
                                double ww = hh / 2;
                                double minx = Math.Min(head2d.x, origin2d.x);
                                double maxx = Math.Max(head2d.x, origin2d.x);
                                bb_min = new Vec2(minx - (ww / 2), head2d.y - r);
                                bb_max = new Vec2(maxx + (ww / 2), origin2d.y + r);
                            }

                        }
                        if (bb_min == null)
                            continue; //if all bones failed to w2s but origin was visible, yes this is possible and will crash

                        int h = (int)bb_max.y - (int)bb_min.y;
                        int w = (int)bb_max.x - (int)bb_min.x;

                        if (settings.esp_box != EspBoxMode.Off)
                        {
                            bool flip_box = settings.esp_vis_mode == EspVisCheck.IndicateVisible && (GameManager.PlayerFlags[i] & PlayerFlags.Visible) != 0;
                            DrawRect((int)bb_min.x - 1, (int)bb_min.y - 1, w + 2, h + 2, 3, flip_box ? color : Color.black);
                            DrawRect((int)bb_min.x, (int)bb_min.y, w, h, 1, flip_box ? Color.grey : color);
                        }
                        if (settings.esp_health_bar)
                        {
                            int hp_h = (int)Util.Map(hp, 0, Player.GetMaxHealth(player), 0, h);
                            int hp_h_t = (int)Util.Map(hp, 0, Player.GetMaxHealth(player), h - 13, 0);
                            DrawRectFilled((int)bb_min.x - 6, (int)bb_min.y - 1, 4, h + 2, Color.black);
                            DrawRectFilled((int)bb_min.x - 5, (int)bb_min.y + (h - hp_h), 2, hp_h, Color.green);
                            if (settings.esp_health_num)
                                DrawTextOutlined(hp.ToString("0.#"), (int)bb_min.x - 7, (int)(bb_min.y + hp_h_t), Color.white, TextAlign.RIGHT | TextAlign.TOP);
                        }
                        else if (settings.esp_health_num)
                            DrawTextOutlined(hp.ToString("0.#"), (int)bb_min.x - 7, (int)(bb_min.y), Color.white, TextAlign.RIGHT | TextAlign.TOP);

                        if (settings.esp_name)
                            DrawTextOutlined(Player.GetName(player), (int)bb_min.x + (w / 2), (int)(bb_min.y) - 5, color, TextAlign.BOTTOM | TextAlign.H_CENTER);

                        if (settings.debug_esp)
                        {
                            Vec3 shhot_vec = CollisionHelper.GetShootVectorBase(current);
                            Vec3 head = Player.GetHeadPos(current);

                            double dist = Math3D.DistanceToSegment(BoneManager.HeadBones[i].tail, head, head + shhot_vec);
                            DrawText("dist " + dist.ToString("0.##"), (int)bb_max.x + 4, (int)bb_min.y);
                        } 
                        else if (settings.esp_flags)
                        {
                            int fx = (int)bb_max.x + 4;
                            int fy = (int)bb_min.y;
                            int fi = 0;
                            int fii = fonts[0].Size + 2;
                            if (Player.IsScopped(player))  DrawTextOutlined("SCOPPED", fx, fy + (fi++ * fii), Color.white);
                            if (Player.GetCrouchProgress(player) > 0) DrawTextOutlined("CROUCHED", fx, fy + (fi++ * fii), Color.white);
                            if (Aimbot.lockedTarget == i)  DrawTextOutlined("LOCKED", fx, fy + (fi++ * fii), Color.blue);
                            if (Aimbot.currentTargetID == i)  DrawTextOutlined("AIMBOT", fx, fy + (fi++ * fii), Color.green);
                        }

                        if (settings.esp_snaplines)
                            DrawLine((float)GameManager.ScreenResolution.x / 2, (float)GameManager.ScreenResolution.y,
                                settings.esp_box != EspBoxMode.Off ? (float)(bb_min.x + (w / 2)) : (float)origin2d.x,
                                settings.esp_box != EspBoxMode.Off ? (float)bb_max.y : (float)origin2d.y, 1, color);
                    }
                }

            }


        }

        internal static void DrawProjectiles()
        {
            object gm = GameManager.instance.Target;

            List<object> ents = GameManager.CollisionEntitys(gm);

            int i = 0;
            foreach (object e in ents)
            {
                Type et = e.GetType();
                
                if ((et == SignatureManager.Grenade.Type && settings.esp_grenade) 
                    || (et == SignatureManager.GLauncher.Type && settings.esp_grenade_launcher))
                {
                    Vec3 pos = CollisionEntity.GetPosition(e);
                    object toolType = CollisionEntity.GetToolRaw(e);
                    bool isGL = (byte)toolType == (byte)ToolType.GLauncher;
                    int life;
                    double ep = 0;
                    if (isGL)
                    {
                        ep = CollisionEntity.GetBounceWatchProgress(e);
                        life = (int)(ep * 3000);
                    }
                    else
                        life = CollisionEntity.GetLifetime(e);
                    object player = GameManager.GetPlayerByID(gm, CollisionEntity.GetOwnerID(e));
                    Color color = player != null ? (player == GameManager.GetLocalPLayer(gm) ? Color.white :
                        GameManager.GetPlayerColor(gm, player, false)) : Color.white;
                    if (GameManager.W2s(pos, out Vec2 pos2d))
                    {
                        int w = 30;
                        int life_w = (int)Util.Map(life, 0, 3000, 0, w);

                        DrawTextOutlined("Grenade", (int)pos2d.x, (int)pos2d.y - 2, color, TextAlign.BOTTOM | TextAlign.H_CENTER);

                        DrawRectFilled((int)pos2d.x - (w / 2) - 1, (int)pos2d.y,     w + 2,  3, Color.black);
                        DrawRectFilled((int)pos2d.x - (w / 2),     (int)pos2d.y + 1, life_w, 1, color);
                    }
                    {
                        int trace_life = isGL ? (ep == 1.0 ? int.MaxValue : (int)(ep * 750)) : life;
                        DrawGrenade(gm, pos, CollisionEntity.GetVelocity(e), toolType, trace_life, color, 5, 50);
                    }
                }
                else if (et == SignatureManager.C4.Type && (settings.esp_c4 || settings.esp_disruptor))
                {
                    Vec3 pos = CollisionEntity.GetPosition(e, true);
                    if (GameManager.W2s(pos, out Vec2 pos2d))
                    {
                        bool isC4 = CollisionEntity.GetTool(e) == ToolType.C4;
                        if((isC4 && settings.esp_c4) || (!isC4 && settings.esp_disruptor))
                        {
                            object player = GameManager.GetPlayerByID(gm, CollisionEntity.GetOwnerID(e));
                            Color color = player != null ? (player == GameManager.GetLocalPLayer(gm) ? Color.white :
                                GameManager.GetPlayerColor(gm, player, false)) : Color.white;

                            DrawTextOutlined(isC4 ? "C4" : "Disruptor", (int)pos2d.x, (int)pos2d.y, color, TextAlign.CENTER);
                        }
                    }
                }
                else if (et == SignatureManager.Scanner.Type && settings.esp_scanner)
                {
                    Vec3 pos = CollisionEntity.GetPosition(e);
                    if (GameManager.W2s(pos, out Vec2 pos2d))
                    {
                        int life = CollisionEntity.GetLifetime(e);
                        double hp = CollisionEntity.GetHealth(e);
                        int w = 40;
                        int life_w = (int)Util.Map(life, 0, 20000, 0, w);
                        int hp_w = (int)Util.Map(hp, 0, 30, 0, w);

                        object player = GameManager.GetPlayerByID(gm, CollisionEntity.GetOwnerID(e));
                        Color color = player != null ? (player == GameManager.GetLocalPLayer(gm) ? Color.white :
                            GameManager.GetPlayerColor(gm, player, false)) : Color.white;

                        DrawRectFilled((int)pos2d.x - (w / 2) - 1, (int)pos2d.y - 11, w + 2, 3, Color.black);
                        DrawRectFilled((int)pos2d.x - (w / 2), (int)pos2d.y - 10, hp_w, 1, Color.red);

                        DrawTextOutlined("Scanner", (int)pos2d.x, (int)pos2d.y - 2, color, TextAlign.CENTER);

                        DrawRectFilled((int)pos2d.x - (w / 2) - 1, (int)pos2d.y + 9, w + 2, 3, Color.black);
                        DrawRectFilled((int)pos2d.x - (w / 2), (int)pos2d.y + 10, life_w, 1, color);
                    }
                }
                i++;
            }

        }

        public static void DrawGrenade(object gm, Vec3 start, Vec3 startVelocity, object toolType, double life, Color color, int simulationStep, int drawingStep)
        {
            if (drawingStep < simulationStep || drawingStep % simulationStep != 0)
                throw new ArgumentException("drawingStep must not be lower than simulationStep and must be divisible by simulationStep!");
            object map = GameManager.GetMap(gm);
            bool lastLineVisible = GameManager.W2s(start, out Vec2 lastSim_pos);
            Vec3 sim_pos = start;
            Vec3 velocity = startVelocity;
            bool isGL = (byte)toolType == (byte)ToolType.GLauncher;
            for (double t = 0; t < life; t += Math.Max(Math.Min(simulationStep, life - t - 1), 1))
            {
                object trace = CollisionHelper.TraceProjectile(map, toolType, sim_pos, velocity * simulationStep);
                Vec3 bounce_pos = null;
                if (CollisionResult.DidHitWall(trace))
                {
                    Vec3 bounce = CollisionResult.GetBounceVector(trace);
                    if (isGL)
                    {
                        velocity = velocity.Dot(bounce);
                        if (life == int.MaxValue)
                            life = t + 750;
                    }
                    else
                        velocity = velocity.Dot(bounce * 0.33);
                    bounce_pos = sim_pos += velocity * simulationStep;
                }
                else
                {
                    if (isGL)
                    {
                        sim_pos += velocity * simulationStep;
                        velocity.y += -0.000025 * simulationStep;
                    }
                    else
                    {
                        velocity.y += -0.000025 * simulationStep;
                        sim_pos += velocity * simulationStep;
                    }
                }
                if ((bounce_pos != null && GameManager.W2s(bounce_pos, out Vec2 sim_pos2d)) || ((t % drawingStep == 0 || life - t - 1 < simulationStep) && GameManager.W2s(sim_pos, out sim_pos2d)))
                {
                    if(lastLineVisible)
                        DrawLine(lastSim_pos.x, lastSim_pos.y, sim_pos2d.x, sim_pos2d.y, 1, color);
                    lastLineVisible = true;
                    lastSim_pos = sim_pos2d;
                }
            }
        }
    }
}
