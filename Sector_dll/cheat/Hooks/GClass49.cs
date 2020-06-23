using Sector_dll.sdk;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.cheat.Hooks
{
    class GClass49
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void vmethod_4(Action<object, object> orig, object self, object p1)
        {
            object font = Font.New(Font.GameFont, 18, 400);
            object hp_color = Color.New(0, 255, 0);

            //object localPlayer = GameManager.GetLocalPLayer(self);
            //if(localPlayer != null)
            //{
            //    double hp = Player.GetHealth(localPlayer);
            //    DrawingHelper.DrawString(p1, hp.ToString(), Rect.New(100, 100, 200, 200), font, Color.New(0, 255, 0), DrawingHelper.Gravity.TopRight);
            //}

            //DrawingHelper.DrawFilledRect(p1, Rect.New(20, 20, 20, 20), Color.New(255, 0, 0));

            List<object> players = (SignatureManager.GClass49_player_list.GetValue(self) as IEnumerable<object>).Cast<object>().ToList();

            for(int i = 0; i < players.Count; i++)
            {
                object player = players[i];

                if(Player.GetHealth(player) > 0.0)
                {
                    Vec3 origin = Player.GetOrigin(player);
                    Vec3 head = Player.GetHeadPos(player);

                    if (GameManager.W2s(self, origin, out Vec2 origin2d) && GameManager.W2s(self, head, out Vec2 head2d))
                    {
                        double h = origin2d.y - head2d.y;
                        double w = h / 1.7;
                        object color = GameManager.GetPlayerColor(self, player);
                        double hp = Player.GetHealth(player);

                        double hp_h = Util.Map(hp, 0, Player.GetMaxHealth(player), 0, h);


                        object rect = Rect.New(Vec2.New(head2d.x - (w / 2), head2d.y), Size.New(w, h));
                        DrawingHelper.DrawRect(p1, rect, 1.0, color);
                        DrawingHelper.DrawFilledRect(p1, Rect.New(
                            head2d.x - (w / 2) - 3, head2d.y + (h - hp_h),
                            2, hp_h), hp_color);

                        //DrawingHelper.DrawString(p1, P.ToString(),
                        //    Rect.New(head2d.x, head2d.y + 40, 100, 100), font, hp_color, DrawingHelper.Gravity.TopLeft);
                    }
                }

            }
            //Log.Info(xd.Count.ToString());

            orig(self, p1);
        }
    }
}
