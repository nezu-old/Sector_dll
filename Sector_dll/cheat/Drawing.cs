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

            public DrawMenuDelegate DrawMenu;

            public DrawRectDelegate DrawRect;

            public DrawFilledRectDelegate DrawFilledRect;

            public DrawLineDelegate DrawLine;

            public DrawTextDelegate DrawText;

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
                    //d.DrawText(o.ToString(), 100, 50, 20, Color.red, 0);
                    //Player.SetTeam(local, TeamType.Spectate);
                }
                List<object> players = (SignatureManager.GClass49_player_list.GetValue(gm) as IEnumerable<object>)
                    .Cast<object>().ToList();



                //CollisionHelper.GetBonesWorldSpace(local);

                for(int i = 0; i < players.Count; i++)
                {
                    object player = players[i];

                    if(Player.GetHealth(player) > 0.0)
                    {
                        Vec3 origin = Player.GetOrigin(player);
                        Vec3 head = Player.GetHeadPos(player);
                        origin.y -= 0.2;
                        head.y += 0.2;

                        if (GameManager.W2s(origin, out Vec2 origin2d) && GameManager.W2s(head, out Vec2 head2d))
                        {

                            int h = (int)(origin2d.y - head2d.y);
                            int w = (int)(h / 1.65 + Math.Abs(head2d.x - origin2d.x));
                            Color color = GameManager.GetPlayerColor(gm, player);
                            double hp = Player.GetHealth(player);
                            int hp_h = (int)Util.Map(hp, 0, Player.GetMaxHealth(player), 0, h);


                            d.DrawRect((int)head2d.x - (w / 2), (int)head2d.y, w, h, 3, Color.black);
                            d.DrawRect((int)head2d.x - (w / 2), (int)head2d.y, w, h, 1, color);
                            d.DrawFilledRect((int)head2d.x - (w / 2) - 6, (int)head2d.y - 1, 4, h + 2, Color.black);
                            d.DrawFilledRect((int)head2d.x - (w / 2) - 5, (int)head2d.y + (h - hp_h), 2, hp_h, Color.green);

                            d.DrawLine((float)GameManager.ScreenResolution.x / 2, (float)GameManager.ScreenResolution.y,
                                (float)((head2d.x + origin2d.x) / 2), (float)origin2d.y, 1, color);

                            d.DrawText(Player.GetName(player), (float)head2d.x, (float)head2d.y - 5, 18, Color.white, 
                                DrawingFunctions.TextAlignment.ALIGN_BOTTOM | DrawingFunctions.TextAlignment.ALIGN_HCENTER);
                        }

                        byte skinType = Player.GetSkinId(player);
                        TeamType team = Player.GetTeam(player);

                        List<object> bones = NormalBones[skinType, (int)team];
                        if(bones == null)
                        {
                            bones = new List<object>();
                            List<object> all_bones = Bones.GetBoneList(Player.GetBones(player));
                            foreach(object bone in all_bones)
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
                        double offset = (1.22 * scale) + 0.2;

                        Matrix4 matrix = Matrix4.CreateRotationX(-1.5707963267948966) * Matrix4.CreateScale(-scale, scale, scale) 
                            * Matrix4.CreateTranslation(origin + new Vec3(0, offset, 0));

                        for (int j = 0; j < bones.Count(); j++)
                        {
                            object bone = bones[j];
                            Matrix4 final_transform = new Matrix4(transforms[j]) * matrix;
                            if (GameManager.W2s(final_transform * Bone.GetHead(bone), out Vec2 head_b)
                                && GameManager.W2s(final_transform * Bone.GetTail(bone), out Vec2 tail_b))
                            {
                                d.DrawLine((int)head_b.x, (int)head_b.y, (int)tail_b.x, (int)tail_b.y, 1, new Color(255, 0, 255));
                                //d.DrawText(Bone.GetName(bone), (float)head_b.x, (float)head_b.y, 18, Color.white, 
                                //    DrawingFunctions.TextAlignment.ALIGN_CENTER);
                            }
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
