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

                d.DrawText(Config.settings.debug.ToString(), 100, 50, 20, Color.red, 0);




                int k = 0;

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
                            //object bones = CollisionHelper.GetBonesWorldSpace(player);
                            //List<object> bb = (bones as IEnumerable<object>).Cast<object>().ToList();
                            //foreach (var b in bb)
                            //{
                            //    if (GameManager.W2s(WorldSpaceBone.GetHead(b), out Vec2 bh) &&
                            //        GameManager.W2s(WorldSpaceBone.GetTail(b), out Vec2 bt))
                            //    {
                            //        d.DrawLine((int)bh.x, (int)bh.y, (int)bt.x, (int)bt.y, 1, Color.white);
                            //        //if (k == 0)
                            //        //{
                            //        //    int xxx = (WorldSpaceBone.GetID(b) / 30);
                            //        //    d.DrawText(WorldSpaceBone.GetID(b) + " - " + WorldSpaceBone.GetName(b) + "("
                            //        //        + WorldSpaceBone.GetType(b).ToString() + ", " + WorldSpaceBone.GetRadius(b) + ")",
                            //        //        100 + (300 * xxx), 100 + ((j++ % 30) * 20), 20, Color.white, 0);
                            //        //}
                            //    }
                            //}




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

                        try
                        {
                            Vec3 off = new Vec3(170 + (k * 10), 35, 20);

                            object bones_obj = Player.GetBones(player);
                            List<object> bones = Bones.GetBoneList(bones_obj);
                            object[] transforms = Player.GetBoneTransforms(player);

                            if (GameManager.W2s(off + new Vec3(0, 10, 0), out Vec2 bones_pos))
                                d.DrawText(i + " - b: " + bones.Count() + ", t: " + transforms.Length,
                                    (float)bones_pos.x, (float)bones_pos.y, 20, Color.white, DrawingFunctions.TextAlignment.ALIGN_CENTER);

                            //0x00 - 1.82
                            //0x01 - 1.82
                            //0x02 - 1.82
                            //0x03 - 1.82
                            //0x04 - 1.83
                            //0x05 - 1.82
                            //0x06 - 69
                            //0x07 - 1.86
                            //0x08 - 1.82

                            double bb_h = 1.86;// Config.settings.debug;//Bones.GetBBMax(bones_obj).y - Bones.GetBBMin(bones_obj).y;
                            double num3 = (2.7 - 0.15) / bb_h;
                            double offset = /*GameManager.OtherPlayerYOffset(player)*/(1.22 * num3) + 0.2;

                            Matrix4 matrix = Matrix4.CreateRotationX(-1.5707963267948966) * Matrix4.CreateScale(-num3, num3, num3) 
                                * Matrix4.CreateTranslation(origin + new Vec3(0, offset, 0));

                            for (int j = 0; j < Math.Min(bones.Count(), 999); j++)
                            {
                                object bone = bones[j];
                                Matrix4 trans =  new Matrix4(transforms[j]) * matrix;
                                Vec3 hh = trans * Bone.GetHead(bone);
                                Vec3 tt = trans * Bone.GetTail(bone);
                                if (GameManager.W2s(hh, out Vec2 head_b)
                                    && GameManager.W2s(tt, out Vec2 tail_b) &&
                                    !Bone.GetName(bone).ToLower().Contains("control") && !Bone.GetName(bone).ToLower().Contains("contol"))
                                {
                                    d.DrawLine((int)head_b.x, (int)head_b.y, (int)tail_b.x, (int)tail_b.y, 1, new Color(255, 0, 255));
                                    //d.DrawText(Bone.GetName(bone), (float)head_b.x, (float)head_b.y, 18, Color.white, 
                                    //    DrawingFunctions.TextAlignment.ALIGN_CENTER);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Log.Danger(ex.ToString());
                        }
                        k++;
                    }

                }

                if (players.Count < 1)
                {
                    object pp = Player.New(gm, 10);
                    Player.SetTeam(pp, TeamType.Helix);
                    Player.SetOrigin(pp, new Vec3(165.0, 35.0, 38.0));

                    
                    object skin = pp.GetType().GetField("#=zxt8e6rUza4fF").GetValue(pp);
                    FieldInfo ffi = skin.GetType().GetField("#=zBPULLK9USWK8");
                    ffi.SetValue(skin, Enum.ToObject(ffi.FieldType, 0x7));
                    FieldInfo ffi2 = skin.GetType().GetField("#=zFmQ_Mxo=");
                    ffi2.SetValue(skin, Enum.ToObject(ffi2.FieldType, 0x2));
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
