using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sector_dll.cheat.Hooks
{
    class GClass49
    {

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        private static Vec3 box = new Vec3(64, 16, 64);

        public static double xdad = 3;

        private static bool debounce = false;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void vmethod_4(Action<object, object> orig, object self, object p1)
        {
            GameManager.NewFrame(self);
            object font = Font.New(Font.GameFont, 18, 400);
            object font_s = Font.New(Font.GameFont, 14, 400);
            object hp_color = Color.New(0, 255, 0);
            object black = Color.New(0, 0, 0);

            object localPlayer = GameManager.GetLocalPLayer(self);
            if(localPlayer != null)
            {
                if (GameManager.W2s(self, box + new Vec3(0, 0, 0), out Vec2 v1) &&
                    GameManager.W2s(self, box + new Vec3(1, 0, 0), out Vec2 v2) &&
                    GameManager.W2s(self, box + new Vec3(0, 0, 1), out Vec2 v3) &&
                    GameManager.W2s(self, box + new Vec3(1, 0, 1), out Vec2 v4) &&
                    GameManager.W2s(self, box + new Vec3(0, 1, 0), out Vec2 v5) &&
                    GameManager.W2s(self, box + new Vec3(1, 1, 0), out Vec2 v6) &&
                    GameManager.W2s(self, box + new Vec3(0, 1, 1), out Vec2 v7) &&
                    GameManager.W2s(self, box + new Vec3(1, 1, 1), out Vec2 v8))
                {
                    DrawingHelper.DrawFilledRect(p1, Rect.New(v1.x - 2, v1.y - 2, 4, 4), hp_color);
                    DrawingHelper.DrawFilledRect(p1, Rect.New(v2.x - 2, v2.y - 2, 4, 4), hp_color);
                    DrawingHelper.DrawFilledRect(p1, Rect.New(v3.x - 2, v3.y - 2, 4, 4), hp_color);
                    DrawingHelper.DrawFilledRect(p1, Rect.New(v4.x - 2, v4.y - 2, 4, 4), hp_color);
                    DrawingHelper.DrawFilledRect(p1, Rect.New(v5.x - 2, v5.y - 2, 4, 4), hp_color);
                    DrawingHelper.DrawFilledRect(p1, Rect.New(v6.x - 2, v6.y - 2, 4, 4), hp_color);
                    DrawingHelper.DrawFilledRect(p1, Rect.New(v7.x - 2, v7.y - 2, 4, 4), hp_color);
                    DrawingHelper.DrawFilledRect(p1, Rect.New(v8.x - 2, v8.y - 2, 4, 4), hp_color);
                }
                if (GetAsyncKeyState(0x05) != 0) {
                    box = Player.GetHeadPos(localPlayer);
                    box.x = Math.Floor(box.x - 0.5);
                    box.y = Math.Floor(box.y - 0.5);
                    box.z = Math.Floor(box.z - 0.5);
                }


                if (GetAsyncKeyState(0x06) != 0 && !debounce)
                {
                    xdad += 0.1;
                }
                debounce = GetAsyncKeyState(0x06) != 0;
            }

            //int db = 0;

            //object curPlayer = GameManager.GetCurrentPLayer(self);

            //if (curPlayer != null)
            //{
            //    DrawingHelper.DrawString(p1,
            //        Player.GetCurrentWeaponIndex(curPlayer).ToString(),
            //        Rect.New(100, 100 + (db++ * 30), 500, 200), font, Color.New(0, 255, 0), DrawingHelper.Gravity.TopRight);

            //    DrawingHelper.DrawString(p1,
            //        "wt: " + Player.GetCurrentWeaponType(curPlayer),
            //        Rect.New(100, 100 + (db++ * 30), 500, 200), font, Color.New(0, 255, 0), DrawingHelper.Gravity.TopRight);

            //    DrawingHelper.DrawString(p1,
            //        "st: " + ((byte)Helper.GetEquippedScopeType(curPlayer, Player.GetCurrentWeaponType(curPlayer))),
            //        Rect.New(100, 100 + (db++ * 30), 500, 200), font, Color.New(0, 255, 0), DrawingHelper.Gravity.TopRight);
            //}
            //DrawingHelper.DrawString(p1, 
            //    SignatureManager.GClass49.Type.BaseType
            //    .GetMethod("#=zN_KNRFaSgycA", BindingFlags.Public | BindingFlags.Instance).Invoke(self, new object[] { })?.ToString() ?? "[NULL]", 
            //    Rect.New(100, 100 + (db++ * 30), 500, 200), font, Color.New(0, 255, 0), DrawingHelper.Gravity.TopRight);

            //object thing = SignatureManager.GClass49.Type.BaseType
            //    .GetField("#=z2ykBAQyB7D6a", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            //if(thing != null)
            //{
            //    int xx = (int)thing.GetType().GetField("#=zkBbIjZQ=", BindingFlags.Public | BindingFlags.Instance).GetValue(thing);
            //    int yy = (int)thing.GetType().GetField("#=zwdM35Yg=", BindingFlags.Public | BindingFlags.Instance).GetValue(thing);
            //    DrawingHelper.DrawString(p1,
            //        string.Format("{0}:{1}", xx, yy),
            //        Rect.New(100, 100 + (db++ * 30), 500, 200), font, Color.New(0, 255, 0), DrawingHelper.Gravity.TopRight);
            //}

            //DrawingHelper.DrawString(p1,
            //        xdad.ToString(),
            //        Rect.New(100, 100 + (db++ * 30), 500, 200), font, Color.New(0, 255, 0), DrawingHelper.Gravity.TopRight);

            //DrawingHelper.DrawString(p1,
            //        "fov: " + Settings.GetFov(GameManager.GetSettings(self)),
            //        Rect.New(100, 100 + (db++ * 30), 500, 200), font, Color.New(0, 255, 0), DrawingHelper.Gravity.TopRight);

            //DrawingHelper.DrawFilledRect(p1, Rect.New(20, 20, 20, 20), Color.New(255, 0, 0));

            List<object> players = (SignatureManager.GClass49_player_list.GetValue(self) as IEnumerable<object>).Cast<object>().ToList();

            for(int i = 0; i < players.Count; i++)
            {
                object player = players[i];

                if(Player.GetHealth(player) > 0.0)
                {
                    Vec3 origin = Player.GetOrigin(player);
                    Vec3 head = Player.GetHeadPos(player);
                    origin.y -= 0.2;
                    head.y += 0.2;

                    if (GameManager.W2s(self, origin, out Vec2 origin2d) && GameManager.W2s(self, head, out Vec2 head2d))
                    {
                        double h = origin2d.y - head2d.y;
                        double w = h / 1.65 + Math.Abs(head2d.x - origin2d.x);
                        object color = GameManager.GetPlayerColor(self, player);
                        double hp = Player.GetHealth(player);
                        double hp_h = Util.Map(hp, 0, Player.GetMaxHealth(player), 0, h);


                        DrawingHelper.DrawRect(p1, Rect.New(head2d.x - (w / 2) - 1, head2d.y - 1, w + 2, h + 2), 3.0, black);
                        DrawingHelper.DrawRect(p1, Rect.New(head2d.x - (w / 2), head2d.y, w, h), 1.0, color);
                        DrawingHelper.DrawFilledRect(p1, Rect.New(head2d.x - (w / 2) - 6, head2d.y - 1, 4, h + 2), black);
                        DrawingHelper.DrawFilledRect(p1, Rect.New(head2d.x - (w / 2) - 5, head2d.y + (h - hp_h), 2, hp_h), hp_color);

                        DrawingHelper.DrawString(p1, Player.GetName(player),
                            Rect.New(head2d.x - 200, head2d.y - 20, 400, 30), font, color, DrawingHelper.Gravity.TopCenter);

                        //List<object> bones = (CollisionHelper.GetBonesWorldSpaceClient(Player.GenerateHistoryPlayer(player), GameManager.GetMap(self))
                        //     as IEnumerable<object>).Cast<object>().ToList();

                        ////Vec2 bone_start = new Vec2(head2d.x + w + 5, head2d.y);

                        //for (int j = 0; j < bones.Count; j++)
                        //{
                        //    object bone = bones[j];
                        //    if(GameManager.W2s(self, new Vec3(WorldSpaceBone.GetHead(bone)), out Vec2 b2d) &&
                        //        GameManager.W2s(self, new Vec3(WorldSpaceBone.GetTail(bone)), out Vec2 bt2d) &&
                        //        WorldSpaceBone.GetType(bone).ToString() != "14")
                        //    {
                        //        DrawingHelper.DrawFilledRect(p1, Rect.New(b2d.x - 2, b2d.y - 2, 4, 4), Color.New(0xFF0000FF));
                        //        DrawingHelper.DrawFilledRect(p1, Rect.New(bt2d.x - 2, bt2d.y - 2, 4, 4), Color.New(0xFF0000FF));
                        //        //DrawingHelper.DrawString(p1, ".",
                        //        //    Rect.New(b2d.x - 400, b2d.y - 200, 800, 400), font_s, Color.New(0xFFFFFFFF), DrawingHelper.Gravity.Center);
                        //        //DrawingHelper.DrawString(p1, ".",
                        //        //    Rect.New(bt2d.x - 400, bt2d.y - 200, 800, 400), font_s, Color.New(0xFFFFFFFF), DrawingHelper.Gravity.Center);
                        //    }
                        //}
                    }
                }

            }
            //Log.Info(xd.Count.ToString());

            orig(self, p1);
        }
    }
}
