using Sector_dll.sdk;
using Sector_dll.util;
using sectorsedge.cheat.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sector_dll.cheat.Drawing;
using static Sector_dll.cheat.Config;
using System.Configuration;

namespace Sector_dll.cheat
{
    class ESP
    {

        internal static void DrawPlayerEsp()
        {
            object gm = GameManager.instance.Target;

            object local = GameManager.GetLocalPLayer(gm);
            if (settings.esp_mode == EspModes.Off ||
                (settings.esp_mode == EspModes.OnDeath && (local != null && Player.GetHealth(local) > 0)))
                return;

            byte local_team = local != null ? (byte)Player.GetTeam(local) : (byte)0xFF;

            List<object> players = GameManager.GetPlayers(gm);

            DrawText(GameManager.W2SResolution.ToString(), 100, 80, Color.white);
            GameManager.W2s(new Vec3(0, 0, 0), out Vec2 test);
            {
                DrawText(test.ToString(), 100, 50, Color.white);
                DrawRect((int)test.x - 5, (int)test.y - 5, 10, 10, 2, Color.red);
            }

            for (int i = 0; i < players.Count; i++)
            {
                object player = players[i];

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

                        BoneManager.SetupBones(player, i);
                        List<WorldSpaceBone> bones = BoneManager.BoneCache[i];

                        Vec2 bb_min = null;
                        Vec2 bb_max = null;
                        for (int j = 0; j < bones.Count(); j++)
                        {
                            WorldSpaceBone bone = bones[j];

                            if (GameManager.W2s(bone.head, out Vec2 head_b)
                                && GameManager.W2s(bone.tail, out Vec2 tail_b))
                            {
                                double r = dist_scale * bone.radius * 2;

                                double min_x = Math.Min(tail_b.x, head_b.x) - r;
                                double min_y = Math.Min(tail_b.y, head_b.y) - r;
                                double max_x = Math.Max(tail_b.x, head_b.x) + r;
                                double max_y = Math.Max(tail_b.y, head_b.y) + r;
                                if (bb_min == null)
                                {
                                    bb_min = new Vec2(min_x, min_y);
                                    bb_max = new Vec2(max_x, max_y);
                                }
                                if (min_x < bb_min.x) bb_min.x = min_x;
                                if (min_y < bb_min.y) bb_min.y = min_y;
                                if (max_x > bb_max.x) bb_max.x = max_x;
                                if (max_y > bb_max.y) bb_max.y = max_y;

                                if (Config.settings.esp_skeleton)
                                    DrawLine((int)head_b.x, (int)head_b.y, (int)tail_b.x, (int)tail_b.y, 1, Color.white);
                                //d.DrawText(Bone.GetName(bone), (float)head_b.x, (float)head_b.y, 18, Color.white,
                                //    DrawingFunctions.TextAlignment.ALIGN_CENTER);
                            }
                        }
                        if (bb_min == null)
                            continue; //if all bones faild to w2s but origin was visible, yes this is possible and will crash

                        int h = (int)(bb_max.y - bb_min.y);
                        int w = (int)(bb_max.x - bb_min.x);
                        Color color = GameManager.GetPlayerColor(gm, player, false);

                        if (settings.esp_box)
                        {
                            DrawRect((int)bb_min.x - 1, (int)bb_min.y - 1, w + 2, h + 2, 3, Color.black);
                            DrawRect((int)bb_min.x, (int)bb_min.y, w, h, 1, color);
                        }
                        if (settings.esp_health_bar)
                        {
                            int hp_h = (int)Util.Map(hp, 0, Player.GetMaxHealth(player), 0, h);
                            int hp_h_t = (int)Util.Map(hp, 0, Player.GetMaxHealth(player), h - 13, 0);
                            DrawRectFilled((int)bb_min.x - 6, (int)bb_min.y - 1, 4, h + 2, Color.black);
                            DrawRectFilled((int)bb_min.x - 5, (int)bb_min.y + (h - hp_h), 2, hp_h, Color.green);
                            if (settings.esp_health_num)
                                DrawTextOutlined(hp.ToString(), (int)bb_min.x - 7, (int)(bb_min.y + hp_h_t), Color.white, TextAlign.RIGHT | TextAlign.TOP);
                        }
                        else if (settings.esp_health_num)
                            DrawTextOutlined(hp.ToString(), (int)bb_min.x - 7, (int)(bb_min.y), Color.white, TextAlign.RIGHT | TextAlign.TOP);

                        if (settings.esp_name)
                            DrawTextOutlined(Player.GetName(player), (int)bb_min.x + (w / 2), (int)(bb_min.y) - 5, color, TextAlign.BOTTOM | TextAlign.H_CENTER);

                        if (settings.esp_snaplines)
                            DrawLine((float)GameManager.ScreenResolution.x / 2, (float)GameManager.ScreenResolution.y,
                                settings.esp_box ? (float)(bb_min.x + (w / 2)) : (float)origin2d.x,
                                settings.esp_box ? (float)bb_max.y : (float)origin2d.y, 1, color);
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
                //DrawText(et.Name, 100, 100 + (i * 14), Color.green);
                if ((et == SignatureManager.Grenade.Type && settings.esp_grenade) 
                    || (et == SignatureManager.GLauncher.Type && settings.esp_grenade_launcher))
                {
                    Vec3 pos = CollisionEntity.GetPosition(e);
                    if (GameManager.W2s(pos + new Vec3(0, Config.settings.debug1, 0), out Vec2 pos2d))
                    {
                        int life = et == SignatureManager.GLauncher.Type ? (int)(CollisionEntity.GetBounceWatchProgress(e) * 3000)
                            : CollisionEntity.GetLifetime(e);
                        int w = 30;
                        int life_w = (int)Util.Map(life, 0, 3000, 0, w);

                        object player = GameManager.GetPlayerByID(gm, CollisionEntity.GetOwnerID(e));
                        Color color = player != null ? (player == GameManager.GetLocalPLayer(gm) ? Color.white :
                            GameManager.GetPlayerColor(gm, player, false)) : Color.white;

                        DrawTextOutlined("Grenade", (int)pos2d.x, (int)pos2d.y - 2, color, TextAlign.BOTTOM | TextAlign.H_CENTER);

                        DrawRectFilled((int)pos2d.x - (w / 2) - 1, (int)pos2d.y, w + 2, 3, Color.black);
                        DrawRectFilled((int)pos2d.x - (w / 2), (int)pos2d.y + 1, life_w, 1, color);
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
    }
}
