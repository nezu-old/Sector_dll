using RGiesecke.DllExport;
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
            public delegate void DrawTextDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string text, float x, float y, float size, uint color,
                [MarshalAs(UnmanagedType.I4)] TextAlignment alignment);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawTextSmallDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string text, float x, float y, uint color,
                [MarshalAs(UnmanagedType.I4)] TextAlignment alignment);

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
                    d.DrawText(Player.GetPitch(local) + ":" + Player.GetYaw(local), 100, 50, 20, Color.red, 0);
                    //Player.SetTeam(local, TeamType.Spectate);
                }
                List<object> players = (SignatureManager.GClass49_player_list.GetValue(gm) as IEnumerable<object>)
                    .Cast<object>().ToList();

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

                            byte skinType = Player.GetSkinId(player);
                            TeamType team = Player.GetTeam(player);

                            List<object> bones = NormalBones[skinType, (int)team];
                            if (bones == null)
                            {
                                bones = new List<object>();
                                List<object> all_bones = Bones.GetBoneList(Player.GetBones(player));
                                foreach (object bone in all_bones)
                                {
                                    string name = Bone.GetName(bone).ToLower();
                                    if (!name.Contains("control") && !name.Contains("contol") &&
                                        !name.Contains("blade") && !name.Contains("trigger") &&
                                        !name.Contains("weapon"))
                                        bones.Add(bone);
                                }
                                NormalBones[skinType, (int)team] = bones;
                            }

                            object[] transforms = Player.GetBoneTransforms(player);

                            double scale = (2.7 - 0.15) / Bones.GetScaleForSkin(skinType); // player h(2.7) is diffrent(3.7) for infected but that's dead
                            double offset = (1.22 * scale);// + 0.2;

                            Matrix4 matrix = Matrix4.CreateRotationX(-1.5707963267948966) * Matrix4.CreateScale(-scale, scale, scale)
                                * Matrix4.CreateTranslation(origin + new Vec3(0, offset, 0));

                            Vec2 bb_min = null;
                            Vec2 bb_max = null;

                            for (int j = 0; j < bones.Count(); j++)
                            {
                                object bone = bones[j];
                                Matrix4 final_transform = new Matrix4(transforms[j]) * matrix;
                                Vec3 bone_head = final_transform * Bone.GetHead(bone);
                                Vec3 bone_tail = final_transform * Bone.GetTail(bone);
                                if (Bone.IsHead(bone))
                                    bone_tail = Helper.Lerp(bone_head, bone_tail, 0.55);

                                if (GameManager.W2s(bone_head, out Vec2 head_b)
                                    && GameManager.W2s(bone_tail, out Vec2 tail_b))
                                {
                                    double r = dist_scale * Bone.GetRadius(bone) * 2;

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

                            //d.DrawLine((float)bb_min.x, (float)bb_min.y, (float)bb_min.x, (float)(bb_min.y - dist_scale), 2, Color.green);

                        }
                    }

                }

                if (players.Count < 1)
                {
                    object pp = Player.New(gm, 10);
                    Player.SetTeam(pp, TeamType.Helix);
                    Player.SetOrigin(pp, new Vec3(165.0, 35.0, 38.0));

                    
                    object skin = pp.GetType().GetField("#=zxt8e6rUza4fF").GetValue(pp);
                    FieldInfo ffi = skin.GetType().GetField("#=zBPULLK9USWK8");
                    ffi.SetValue(skin, Enum.ToObject(ffi.FieldType, (byte)Config.settings.debug3));
                    FieldInfo ffi2 = skin.GetType().GetField("#=zFmQ_Mxo=");
                    ffi2.SetValue(skin, Enum.ToObject(ffi2.FieldType, (byte)Config.settings.debug4));
                    pp.GetType().GetField("#=zxt8e6rUza4fF").SetValue(pp, skin);

                    object all_pp = SignatureManager.GClass49_player_list.GetValue(gm);
                    SignatureManager.GClass49_player_list.FieldType.GetMethod("Add").Invoke(all_pp, new[] { pp });
                } 
                else
                {
                    //object pp = players[0];

                    //Vec3 xd = Player.GetOrigin(pp);
                    //xd.x += 0.001;
                    //Player.SetOrigin(pp, xd);
                }

            }
        }

    }
}
