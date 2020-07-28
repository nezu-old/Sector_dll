using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat
{
    class ESP
    {

        internal static void DrawPlayerEsp(Drawing.DrawingFunctions d)
        {
            object gm = GameManager.instance.Target;

            object local = GameManager.GetLocalPLayer(gm);
            if (Config.settings.esp_mode == Config.EspModes.Off ||
                (Config.settings.esp_mode == Config.EspModes.OnDeath && (local != null && Player.GetHealth(local) > 0)))
                return;

            byte local_team = local != null ? (byte)Player.GetTeam(local) : (byte)0xFF;

            List<object> players = GameManager.GetPlayers(gm);

            for (int i = 0; i < players.Count; i++)
            {
                object player = players[i];

                byte team = (byte)Player.GetTeam(player);

                if (Config.settings.esp_team != Config.EspTarget.All && (
                    (Config.settings.esp_team == Config.EspTarget.Enemy && team == local_team) ||
                    (Config.settings.esp_team == Config.EspTarget.Team && team != local_team)))
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

                                if (Config.settings.esp_skeleton > 0)
                                    d.DrawLine((int)head_b.x, (int)head_b.y, (int)tail_b.x, (int)tail_b.y, 1, Color.white);
                                //d.DrawText(Bone.GetName(bone), (float)head_b.x, (float)head_b.y, 18, Color.white,
                                //    DrawingFunctions.TextAlignment.ALIGN_CENTER);
                            }
                        }
                        if (bb_min == null)
                            continue; //if all bones faild to w2s but origin was visible, yes this is possible and will crash

                        int h = (int)(bb_max.y - bb_min.y);
                        int w = (int)(bb_max.x - bb_min.x);
                        Color color = GameManager.GetPlayerColor(gm, player);

                        if (Config.settings.esp_box > 0)
                        {
                            d.DrawRect((int)bb_min.x, (int)bb_min.y, w, h, 3, Color.black);
                            d.DrawRect((int)bb_min.x, (int)bb_min.y, w, h, 1, color);
                        }
                        if (Config.settings.esp_health_bar > 0)
                        {
                            int hp_h = (int)Util.Map(hp, 0, Player.GetMaxHealth(player), 0, h);
                            int hp_h_t = (int)Util.Map(hp, 0, Player.GetMaxHealth(player), h - 13, 0);
                            d.DrawFilledRect((int)bb_min.x - 6, (int)bb_min.y - 1, 4, h + 2, Color.black);
                            d.DrawFilledRect((int)bb_min.x - 5, (int)bb_min.y + (h - hp_h), 2, hp_h, Color.green);
                            if (Config.settings.esp_health_num > 0)
                                d.DrawTextSmall(hp.ToString(), (float)bb_min.x - 7, (float)(bb_min.y + hp_h_t), Color.white,
                                    Drawing.DrawingFunctions.TextAlignment.ALIGN_RIGHT | Drawing.DrawingFunctions.TextAlignment.ALIGN_TOP);
                        }
                        else if (Config.settings.esp_health_num > 0)
                        {
                            d.DrawTextSmall(hp.ToString(), (float)bb_min.x - 7, (float)(bb_min.y), Color.white,
                                Drawing.DrawingFunctions.TextAlignment.ALIGN_RIGHT | Drawing.DrawingFunctions.TextAlignment.ALIGN_TOP);
                        }

                        if (Config.settings.esp_name > 0)
                            d.DrawText(Player.GetName(player), (float)bb_min.x + (w / 2), (float)(bb_min.y) - 5, 18, Color.white,
                                Drawing.DrawingFunctions.TextAlignment.ALIGN_BOTTOM | Drawing.DrawingFunctions.TextAlignment.ALIGN_HCENTER);

                        if (Config.settings.esp_snaplines > 0)
                            d.DrawLine((float)GameManager.ScreenResolution.x / 2, (float)GameManager.ScreenResolution.y,
                                Config.settings.esp_box > 0 ? (float)(bb_min.x + (w / 2)) : (float)origin2d.x,
                                Config.settings.esp_box > 0 ? (float)bb_max.y : (float)origin2d.y, 1, color);

                    }
                }

            }


        }

        internal static void DrawProjectiles(Drawing.DrawingFunctions d)
        {
            object gm = GameManager.instance.Target;

            List<object> ents = GameManager.CollisionEntitys(gm);

            int i = 0;
            foreach(object e in ents)
            {

                d.DrawText(e.GetType().ToString(), 100 , 100 + (i++ * 25));

            }


        }
    }
}
