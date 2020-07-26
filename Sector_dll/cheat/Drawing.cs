﻿using RGiesecke.DllExport;
using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat
{
    class Drawing
    {

        private static List<object>[,] NormalBones = new List<object>[30, 2];

        [StructLayout(LayoutKind.Sequential)]
        public struct DrawingFunctions {

            public enum TextAlignment : int
            {
                ALIGN_TOP = 1,
                ALIGN_BOTTOM = 2,
                ALIGN_LEFT = 4,
                ALIGN_RIGHT = 8,
                ALIGN_VCENTER = 16,
                ALIGN_HCENTER = 32,
                ALIGN_CENTER = ALIGN_VCENTER | ALIGN_HCENTER,
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawMenuDelegate(ref Config.Settings settings);
            
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawRectDelegate(int x, int y, int w, int h, int t, uint color);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawFilledRectDelegate(int x, int y, int w, int h, uint color);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawLineDelegate(float x1, float y1, float x2, float y2, float t, uint color);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawTextDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string text, float x, float y, float size = 20, uint color = 0xFFFFFFFF,
                [MarshalAs(UnmanagedType.I4)] TextAlignment alignment = 0);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawTextSmallDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string text, float x, float y, uint color = 0xFFFFFFFF,
                [MarshalAs(UnmanagedType.I4)] TextAlignment alignment = 0);

            public DrawMenuDelegate DrawMenu;

            public DrawRectDelegate DrawRect;

            public DrawFilledRectDelegate DrawFilledRect;

            public DrawLineDelegate DrawLine;

            public DrawTextDelegate DrawText;

            public DrawTextSmallDelegate DrawTextSmall;

        }

        [DllExport("DrawCallback")]
        public static void DrawCallback(ref DrawingFunctions d)
        {
            d.DrawMenu(ref Config.settings);

            if (Config.settings.debug6 > 0) Log.Info("Frame");

            if (GameManager.instance.IsAlive && GameManager.instance.Target.GetType().BaseType == SignatureManager.GClass49.Type.BaseType)
            {
                object gm = GameManager.instance.Target;

                //d.DrawFilledRect(20, 20, 10, 10, 0xFF0000FF);

                object local = GameManager.GetLocalPLayer(gm);
                if(local != null)
                {
                    //Vec3 o = Player.GetOrigin(local);
                    //Player.SetPitch(local, 0);
                    //Player.SetYaw(local, 0);
                    d.DrawText(Player.GetHeadPos(local).ToString(), 100, 50, 20, Color.red, 0);
                    //Player.SetTeam(local, TeamType.Spectate);
                }
                List<object> players = GameManager.GetPlayers(gm);

                for(int i = 0; i < players.Count; i++)
                {
                    object player = players[i];

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
                                    d.DrawLine((int)head_b.x, (int)head_b.y, (int)tail_b.x, (int)tail_b.y, 1, Color.white);
                                    //d.DrawText(Bone.GetName(bone), (float)head_b.x, (float)head_b.y, 18, Color.white,
                                    //    DrawingFunctions.TextAlignment.ALIGN_CENTER);
                                }
                            }
                            if (bb_min == null)
                                continue; //if all bones faild to w2s but origin was visible

                            int h = (int)(bb_max.y - bb_min.y);
                            int w = (int)(bb_max.x - bb_min.x);
                            Color color = GameManager.GetPlayerColor(gm, player);
                            int hp_h = (int)Util.Map(hp, 0, Player.GetMaxHealth(player), 0, h);
                            int hp_h_t = (int)Util.Map(hp, 0, Player.GetMaxHealth(player), h - 13, 0);


                            d.DrawRect((int)bb_min.x, (int)bb_min.y, w, h, 3, Color.black);
                            d.DrawRect((int)bb_min.x, (int)bb_min.y, w, h, 1, color);

                            d.DrawFilledRect((int)bb_min.x - 6, (int)bb_min.y - 1, 4, h + 2, Color.black);
                            d.DrawFilledRect((int)bb_min.x - 5, (int)bb_min.y + (h - hp_h), 2, hp_h, Color.green);
                            d.DrawTextSmall(hp.ToString(), (float)bb_min.x - 7, (float)(bb_min.y + hp_h_t), Color.white,
                                DrawingFunctions.TextAlignment.ALIGN_RIGHT | DrawingFunctions.TextAlignment.ALIGN_TOP);


                            d.DrawLine((float)GameManager.ScreenResolution.x / 2, (float)GameManager.ScreenResolution.y,
                                (float)(bb_min.x + (w / 2)), (float)bb_max.y, 1, color);

                            d.DrawText(Player.GetName(player), (float)bb_min.x + (w / 2), (float)(bb_min.y) - 5, 18, Color.white,
                                DrawingFunctions.TextAlignment.ALIGN_BOTTOM | DrawingFunctions.TextAlignment.ALIGN_HCENTER);

                            WorldSpaceBone head_bone = BoneManager.HeadBones[i];
                            Vec3 headPos = Player.GetHeadPos(player);
                            Vec3 lookingLineEnd = headPos + Player.GetLookAtVector(player);

                            if(GameManager.W2s(headPos, out Vec2 headPos2d) &&
                                GameManager.W2s(lookingLineEnd, out Vec2 lookingLineEnd2d))
                                d.DrawLine((int)headPos2d.x, (int)headPos2d.y, (int)lookingLineEnd2d.x, (int)lookingLineEnd2d.y, 2, color);

                            //d.DrawLine((float)bb_min.x, (float)bb_min.y, (float)bb_min.x, (float)(bb_min.y - dist_scale), 2, Color.green);

                        }
                    }

                }

            }
        }

    }
}
