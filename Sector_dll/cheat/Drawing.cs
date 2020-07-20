using RGiesecke.DllExport;
using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
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
            public delegate void DrawRectDelegate(int x, int y, int w, int h, int t, uint color);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawFilledRectDelegate(int x, int y, int w, int h, uint color);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawLineDelegate(float x1, float y1, float x2, float y2, float t, uint color);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void DrawTextDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string text, float x, float y, float size, uint color,
                [MarshalAs(UnmanagedType.I4)] TextAlignment alignment);

            public DrawRectDelegate DrawRect;

            public DrawFilledRectDelegate DrawFilledRect;

            public DrawLineDelegate DrawLine;

            public DrawTextDelegate DrawText;

        }

        [DllExport("DrawCallback")]
        public static void DrawCallback(ref DrawingFunctions d)
        {
            if (GameManager.instance.IsAlive && GameManager.instance.Target.GetType().BaseType == SignatureManager.GClass49.Type.BaseType)
            {
                object gm = GameManager.instance.Target;

                //d.DrawFilledRect(20, 20, 10, 10, 0xFF0000FF);

                object local = GameManager.GetLocalPLayer(gm);
                if(local != null)
                {
                    Vec3 o = Player.GetOrigin(local);
                    d.DrawText(o.ToString(), 100, 50, 20, Color.red, 0);
                    //Player.SetTeam(local, TeamType.Spectate);
                }


                object inst = gm.GetType().Assembly.GetType("#=zD17ql6wd9AtAXXPCp7syK94=").GetField("#=zpNk0unzUJMzK").GetValue(null);
                object list = inst.GetType().GetField("#=zJWVBAMzb6EbI3UGf7g==").GetValue(inst);
                List<object> list1 = (list as IEnumerable<object>).Cast<object>().ToList();

                FieldInfo getHead = null;
                FieldInfo getTail = null;
                Vec3 off = new Vec3(170, 35, 20);
                foreach (object bone in list1)
                {
                    if(getHead == null)
                    {
                        getHead = bone.GetType().GetField("#=zDFzdGa4=");
                        getTail = bone.GetType().GetField("#=zN7QlMAA=");
                    }
                    if(GameManager.W2s(new Vec3(getHead.GetValue(bone)) + off, out Vec2 head)
                        && GameManager.W2s(new Vec3(getTail.GetValue(bone)) + off, out Vec2 tail))
                    {
                        d.DrawLine((int)head.x, (int)head.y, (int)tail.x, (int)tail.y, 1, Color.white);
                    }
                }

                d.DrawText(list1.Count().ToString(), 100, 100, 20, Color.green, 0);


                //CollisionHelper.GetBonesWorldSpace(local);


                List<object> players = (SignatureManager.GClass49_player_list.GetValue(gm) as IEnumerable<object>)
                    .Cast<object>().ToList();

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
                            object bones = CollisionHelper.GetBonesWorldSpace(player);
                            List<object> bb = (bones as IEnumerable<object>).Cast<object>().ToList();
                            foreach (var b in bb) {
                                if(GameManager.W2s(WorldSpaceBone.GetHead(b), out Vec2 bh) &&
                                    GameManager.W2s(WorldSpaceBone.GetTail(b), out Vec2 bt))
                                {
                                    d.DrawLine((int)bh.x, (int)bh.y, (int)bt.x, (int)bt.y, 1, Color.white);
                                    //if (k == 0)
                                    //{
                                    //    int xxx = (WorldSpaceBone.GetID(b) / 30);
                                    //    d.DrawText(WorldSpaceBone.GetID(b) + " - " + WorldSpaceBone.GetName(b) + "("
                                    //        + WorldSpaceBone.GetType(b).ToString() + ", " + WorldSpaceBone.GetRadius(b) + ")",
                                    //        100 + (300 * xxx), 100 + ((j++ % 30) * 20), 20, Color.white, 0);
                                    //}
                                }
                            }
                            k++;

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
                    }

                }

                if(players.Count < 1)
                {
                    object pp = Player.New(gm, 1);
                    Player.SetTeam(pp, TeamType.Aegis);
                    Player.SetOrigin(pp, new Vec3(165.0, 35.0, 38.0));
                    object all_pp = SignatureManager.GClass49_player_list.GetValue(gm);
                    SignatureManager.GClass49_player_list.FieldType.GetMethod("Add").Invoke(all_pp, new[] { pp });
                } else
                {
                    object pp = players[0];
                    Vec3 xd = Player.GetOrigin(pp);
                    //xd.z += 0.01;
                    Player.SetOrigin(pp, xd);
                }

            }
        }

    }
}
