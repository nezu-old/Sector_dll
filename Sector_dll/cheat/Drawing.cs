using Sector_dll.sdk;
using sectorsedge.cheat;
using sectorsedge.cheat.Drawing;
using sectorsedge.cheat.Drawing.Fonts;
using sectorsedge.sdk;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Management.Instrumentation;
using System.Windows.Forms.VisualStyles;

namespace Sector_dll.cheat
{
    static class Drawing
    {
        public static List<DrawVert> VtxBuffer;
        public static List<int>      IdxBuffer;//we will never exceeed 31 bits so not using uint for simpler maths
        public static List<DrawCmd>  CmdBuffer;
        public static DrawCmd        CurrentDrawCmd;
        public static Vec2f          TexUvWhitePixel = new Vec2f(0.000977f, 0.018519f);

        public static readonly ProggyClean proggyClean = new ProggyClean();

        public static readonly IFont[] fonts = new IFont[]
        {
            proggyClean
        };

        public static int CalcTextWidth(string s, IFont font)
        {
            int total = 0;
            foreach (char c in s.ToCharArray())
                total += (int)font.FindFontGlyph(c).AdvanceX;
            return total;
        }

        public static void DrawText(string s, double x, double y, Color color = null) 
            => DrawText(s, (int)x, (int)y, color);
        public static void DrawTextOutlined(string s, double x, double y, Color color = null) 
            => DrawText(s, (int)x, (int)y, color, TextAlign.DEFAULT, Color.black);
        public static void DrawText(string s, float x, float y, Color color = null) 
            => DrawText(s, (int)x, (int)y, color);
        public static void DrawTextOutlined(string s, float x, float y, Color color = null) 
            => DrawText(s, (int)x, (int)y, color, TextAlign.DEFAULT, Color.black);
        public static void DrawText(string s, int x, int y, Color color = null, TextAlign align = TextAlign.DEFAULT)
            => DrawText(s, x, y, color, align, null);
        public static void DrawTextOutlined(string s, int x, int y, Color color = null, TextAlign align = TextAlign.DEFAULT)
            => DrawText(s, x, y, color, align, Color.black);

        private static void DrawText(string s, int x, int y, Color color, TextAlign align, Color backgroundColor)
        {
            if (color == null) 
                color = Color.white;
            if ((align & TextAlign.H_CENTER) == TextAlign.H_CENTER)
                x -= CalcTextWidth(s, fonts[0]) / 2;
            else if ((align & TextAlign.RIGHT) != 0)
                x -= CalcTextWidth(s, fonts[0]);
            if ((align & TextAlign.V_CENTER) == TextAlign.V_CENTER)
                y -= fonts[0].Size / 2;
            else if ((align & TextAlign.BOTTOM) != 0)
                y -= fonts[0].Size;

            char[] chars = s.ToCharArray();
            int elems = 0;
            foreach (char c in chars)
            {
                FontGlyph glyph = fonts[0].FindFontGlyph(c);
                if (glyph.Visible)
                {
                    float x1 = x + glyph.X0;
                    float x2 = x + glyph.X1;
                    float y1 = y + glyph.Y0;
                    float y2 = y + glyph.Y1;

                    if(backgroundColor != null)
                    {
                        int idx_back = VtxBuffer.Count;
                        VtxBuffer.Add(new DrawVert(x1 + 1, y1 + 1, glyph.U0, glyph.V0, backgroundColor));
                        VtxBuffer.Add(new DrawVert(x2 + 1, y1 + 1, glyph.U1, glyph.V0, backgroundColor));
                        VtxBuffer.Add(new DrawVert(x1 + 1, y2 + 1, glyph.U0, glyph.V1, backgroundColor));
                        VtxBuffer.Add(new DrawVert(x2 + 1, y2 + 1, glyph.U1, glyph.V1, backgroundColor));

                        IdxBuffer.Add(idx_back + 0);
                        IdxBuffer.Add(idx_back + 1);
                        IdxBuffer.Add(idx_back + 2);

                        IdxBuffer.Add(idx_back + 1);
                        IdxBuffer.Add(idx_back + 2);
                        IdxBuffer.Add(idx_back + 3);

                        elems += 6;
                    }
                    int idx = VtxBuffer.Count;
                    VtxBuffer.Add(new DrawVert(x1, y1, glyph.U0, glyph.V0, color));
                    VtxBuffer.Add(new DrawVert(x2, y1, glyph.U1, glyph.V0, color));
                    VtxBuffer.Add(new DrawVert(x1, y2, glyph.U0, glyph.V1, color));
                    VtxBuffer.Add(new DrawVert(x2, y2, glyph.U1, glyph.V1, color));

                    IdxBuffer.Add(idx + 0);
                    IdxBuffer.Add(idx + 1);
                    IdxBuffer.Add(idx + 2);

                    IdxBuffer.Add(idx + 1);
                    IdxBuffer.Add(idx + 2);
                    IdxBuffer.Add(idx + 3);
                    elems += 6;
                }
                x += (int)glyph.AdvanceX;
            }
            CurrentDrawCmd.IncrementElemCount(elems);
        }
        public static VtxIdxPtr ReserveRect(bool filled = true)
        {
            VtxIdxPtr ptr = new VtxIdxPtr(VtxBuffer.Count, IdxBuffer.Count);
            int vtx = 4;
            int idx = 6;
            if(!filled)
            {
                vtx = 8;
                idx = 24;
            }
            VtxBuffer.AddRange(new DrawVert[vtx]);
            IdxBuffer.AddRange(new int[idx]);
            CurrentDrawCmd.IncrementElemCount(idx);
            return ptr;
        }

        public static void DrawRectFilled(int x, int y, int w, int h, Color color, VtxIdxPtr ptr = null)
        {
            float x2 = x + w;
            float y2 = y + h;
            if(ptr == null)
            {
                int idx = VtxBuffer.Count;
                VtxBuffer.Add(new DrawVert(x,  y, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x2, y, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x,  y2, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x2, y2, TexUvWhitePixel.x, TexUvWhitePixel.y, color));

                IdxBuffer.Add(idx + 0);
                IdxBuffer.Add(idx + 1);
                IdxBuffer.Add(idx + 2);

                IdxBuffer.Add(idx + 1);
                IdxBuffer.Add(idx + 2);
                IdxBuffer.Add(idx + 3);

                CurrentDrawCmd.IncrementElemCount(6);
            } 
            else
            {
                int idx = ptr.VtxBufferOffset;
                VtxBuffer[idx + 0] = new DrawVert(x, y, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                VtxBuffer[idx + 1] = new DrawVert(x2, y, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                VtxBuffer[idx + 2] = new DrawVert(x, y2, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                VtxBuffer[idx + 3] = new DrawVert(x2, y2, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                int idx2 = ptr.IdxBufferOffset;
                IdxBuffer[idx2 + 0] = idx + 0;
                IdxBuffer[idx2 + 1] = idx + 1;
                IdxBuffer[idx2 + 2] = idx + 2;

                IdxBuffer[idx2 + 3] = idx + 1;
                IdxBuffer[idx2 + 4] = idx + 2;
                IdxBuffer[idx2 + 5] = idx + 3;
            }
        }

        public static void DrawRect(int x, int y, int w, int h, int t, Color color, VtxIdxPtr ptr = null)
        {
            if(ptr == null)
            {
                int idx = VtxBuffer.Count;

                //outer rect
                VtxBuffer.Add(new DrawVert(x, y, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x + w, y, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x, y + h, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x + w, y + h, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                //inner rect
                VtxBuffer.Add(new DrawVert(x + t, y + t, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x + w - t, y + t, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x + t, y + h - t, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x + w - t, y + h - t, TexUvWhitePixel.x, TexUvWhitePixel.y, color));

                //top 1
                IdxBuffer.Add(idx + 0);
                IdxBuffer.Add(idx + 1);
                IdxBuffer.Add(idx + 4);
                //top2
                IdxBuffer.Add(idx + 1);
                IdxBuffer.Add(idx + 5);
                IdxBuffer.Add(idx + 4);
                //left1
                IdxBuffer.Add(idx + 0);
                IdxBuffer.Add(idx + 4);
                IdxBuffer.Add(idx + 2);
                //left2
                IdxBuffer.Add(idx + 4);
                IdxBuffer.Add(idx + 6);
                IdxBuffer.Add(idx + 2);
                //right1
                IdxBuffer.Add(idx + 1);
                IdxBuffer.Add(idx + 3);
                IdxBuffer.Add(idx + 5);
                //right2
                IdxBuffer.Add(idx + 5);
                IdxBuffer.Add(idx + 3);
                IdxBuffer.Add(idx + 7);
                //bottom1
                IdxBuffer.Add(idx + 2);
                IdxBuffer.Add(idx + 6);
                IdxBuffer.Add(idx + 3);
                //bottom2
                IdxBuffer.Add(idx + 6);
                IdxBuffer.Add(idx + 3);
                IdxBuffer.Add(idx + 7);

                CurrentDrawCmd.IncrementElemCount(24); //8*3
            }
            else
            {
                int idx = ptr.VtxBufferOffset;
                //outer rect
                VtxBuffer[idx + 0] = new DrawVert(x, y, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                VtxBuffer[idx + 1] = new DrawVert(x + w, y, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                VtxBuffer[idx + 2] = new DrawVert(x, y + h, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                VtxBuffer[idx + 3] = new DrawVert(x + w, y + h, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                //inner rect
                VtxBuffer[idx + 4] = new DrawVert(x + t, y + t, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                VtxBuffer[idx + 5] = new DrawVert(x + w - t, y + t, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                VtxBuffer[idx + 6] = new DrawVert(x + t, y + h - t, TexUvWhitePixel.x, TexUvWhitePixel.y, color);
                VtxBuffer[idx + 7] = new DrawVert(x + w - t, y + h - t, TexUvWhitePixel.x, TexUvWhitePixel.y, color);

                int idx2 = ptr.IdxBufferOffset;
                //top 1
                IdxBuffer[idx2 +  0] = idx + 0;
                IdxBuffer[idx2 +  1] = idx + 1;
                IdxBuffer[idx2 +  2] = idx + 4;
                //top2
                IdxBuffer[idx2 +  3] = idx + 1;
                IdxBuffer[idx2 +  4] = idx + 5;
                IdxBuffer[idx2 +  5] = idx + 4;
                //left1
                IdxBuffer[idx2 +  6] = idx + 0;
                IdxBuffer[idx2 +  7] = idx + 4;
                IdxBuffer[idx2 +  8] = idx + 2;
                //left2
                IdxBuffer[idx2 +  9] = idx + 4;
                IdxBuffer[idx2 + 10] = idx + 6;
                IdxBuffer[idx2 + 11] = idx + 2;
                //right1
                IdxBuffer[idx2 + 12] = idx + 1;
                IdxBuffer[idx2 + 13] = idx + 3;
                IdxBuffer[idx2 + 14] = idx + 5;
                //right2
                IdxBuffer[idx2 + 15] = idx + 5;
                IdxBuffer[idx2 + 16] = idx + 3;
                IdxBuffer[idx2 + 17] = idx + 7;
                //bottom1
                IdxBuffer[idx2 + 18] = idx + 2;
                IdxBuffer[idx2 + 19] = idx + 6;
                IdxBuffer[idx2 + 20] = idx + 3;
                //bottom2
                IdxBuffer[idx2 + 21] = idx + 6;
                IdxBuffer[idx2 + 22] = idx + 3;
                IdxBuffer[idx2 + 23] = idx + 7;
            }
        }

        public static void DrawLine(double x1, double y1, double x2, double y2, double t, Color color)
            => DrawLine((float)x1, (float)y1, (float)x2, (float)y2, (float)t, color);

        public static void DrawLine(float x1, float y1, float x2, float y2, float t, Color color)
        {
            uint col_trans = color.color & 0x00FFFFFF;

            float dx = x2 - x1;
            float dy = y2 - y1;

            float d2 = dx * dx + dy * dy; 
            if (d2 > 0.0f) { 
                float inv_len = 1.0f / (float)Math.Sqrt(d2); 
                dx *= inv_len; 
                dy *= inv_len; 
            }
            int idx = VtxBuffer.Count;

            if (t <= 1)
            {
                VtxBuffer.Add(new DrawVert(x1, y1, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x2, y2, TexUvWhitePixel.x, TexUvWhitePixel.y, color));

                VtxBuffer.Add(new DrawVert(x1 + dy, y1 - dx, TexUvWhitePixel.x, TexUvWhitePixel.y, col_trans));
                VtxBuffer.Add(new DrawVert(x2 + dy, y2 - dx, TexUvWhitePixel.x, TexUvWhitePixel.y, col_trans));
                VtxBuffer.Add(new DrawVert(x2 - dy, y2 + dx, TexUvWhitePixel.x, TexUvWhitePixel.y, col_trans));
                VtxBuffer.Add(new DrawVert(x1 - dy, y1 + dx, TexUvWhitePixel.x, TexUvWhitePixel.y, col_trans));

                IdxBuffer.Add(idx + 2);
                IdxBuffer.Add(idx + 3);
                IdxBuffer.Add(idx + 1);

                IdxBuffer.Add(idx + 2);
                IdxBuffer.Add(idx + 1);
                IdxBuffer.Add(idx + 0);

                IdxBuffer.Add(idx + 0);
                IdxBuffer.Add(idx + 1);
                IdxBuffer.Add(idx + 4);

                IdxBuffer.Add(idx + 0);
                IdxBuffer.Add(idx + 4);
                IdxBuffer.Add(idx + 5);

                CurrentDrawCmd.IncrementElemCount(12);
            }
            else
            {
                float half_inner_thickness = (t - 1) * 0.5f;

                float dxo = dx * (half_inner_thickness + 1);
                float dyo = dy * (half_inner_thickness + 1);

                dx *= half_inner_thickness;
                dy *= half_inner_thickness;

                VtxBuffer.Add(new DrawVert(x1 + dy, y1 - dx, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x2 + dy, y2 - dx, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x2 - dy, y2 + dx, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                VtxBuffer.Add(new DrawVert(x1 - dy, y1 + dx, TexUvWhitePixel.x, TexUvWhitePixel.y, color));

                VtxBuffer.Add(new DrawVert(x1 + dyo, y1 - dxo, TexUvWhitePixel.x, TexUvWhitePixel.y, col_trans));
                VtxBuffer.Add(new DrawVert(x2 + dyo, y2 - dxo, TexUvWhitePixel.x, TexUvWhitePixel.y, col_trans));
                VtxBuffer.Add(new DrawVert(x2 - dyo, y2 + dxo, TexUvWhitePixel.x, TexUvWhitePixel.y, col_trans));
                VtxBuffer.Add(new DrawVert(x1 - dyo, y1 + dxo, TexUvWhitePixel.x, TexUvWhitePixel.y, col_trans));

                IdxBuffer.Add(idx + 4);
                IdxBuffer.Add(idx + 5);
                IdxBuffer.Add(idx + 1);

                IdxBuffer.Add(idx + 4);
                IdxBuffer.Add(idx + 1);
                IdxBuffer.Add(idx + 0);

                IdxBuffer.Add(idx + 0);
                IdxBuffer.Add(idx + 1);
                IdxBuffer.Add(idx + 2);

                IdxBuffer.Add(idx + 0);
                IdxBuffer.Add(idx + 2);
                IdxBuffer.Add(idx + 3);

                IdxBuffer.Add(idx + 3);
                IdxBuffer.Add(idx + 2);
                IdxBuffer.Add(idx + 6);

                IdxBuffer.Add(idx + 3);
                IdxBuffer.Add(idx + 6);
                IdxBuffer.Add(idx + 7);

                CurrentDrawCmd.IncrementElemCount(18);
            }
        }

        public static void DrawCircleFilled(float x, float y, float r, int points, Color color, Color centerColor = null)
        {
            float step = (float)Math.PI * 2.0f / points;
            if (centerColor == null) 
                centerColor = color;

            int idx = VtxBuffer.Count;

            VtxBuffer.Add(new DrawVert(x, y, TexUvWhitePixel.x, TexUvWhitePixel.y, centerColor));
            VtxBuffer.Add(new DrawVert(r * (float)Math.Cos(0) + x, r * (float)Math.Sin(0) + y, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
            int i = 2;
            for (float a = step; a < (Math.PI * 2.0f) + step; a += step, i++)
            {
                VtxBuffer.Add(new DrawVert(r * (float)Math.Cos(a) + x, r * (float)Math.Sin(a) + y, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
                IdxBuffer.Add(idx);//center
                IdxBuffer.Add(idx + (i - 1));//previous
                IdxBuffer.Add(idx + i);//current
            }
            CurrentDrawCmd.IncrementElemCount(i*3);
        }

        public static void DrawTexture(int x, int y, int w, int h, Color color)
        {
            int idx = VtxBuffer.Count;
            float x2 = x + w;
            float y2 = y + h;
            VtxBuffer.Add(new DrawVert(x, y, 0, 0, color));
            VtxBuffer.Add(new DrawVert(x2, y, 1, 0, color));
            VtxBuffer.Add(new DrawVert(x, y2, 0, 1, color));
            VtxBuffer.Add(new DrawVert(x2, y2, 1, 1, color));

            IdxBuffer.Add(idx + 0);
            IdxBuffer.Add(idx + 1);
            IdxBuffer.Add(idx + 2);

            IdxBuffer.Add(idx + 1);
            IdxBuffer.Add(idx + 2);
            IdxBuffer.Add(idx + 3);

            CurrentDrawCmd.IncrementElemCount(6);
        }

        public class VtxIdxPtr
        {
            public int VtxBufferOffset;

            public int IdxBufferOffset;

            public VtxIdxPtr(int vtxBufferOffset, int idxBufferOffset)
            {
                VtxBufferOffset = vtxBufferOffset;
                IdxBufferOffset = idxBufferOffset;
            }
        }

        public static void NewFrame()
        {
            VtxBuffer = new List<DrawVert>();
            IdxBuffer = new List<int>();
            CmdBuffer = new List<DrawCmd>();
            CurrentDrawCmd = new DrawCmd(fonts[0].Texture_id);
            CmdBuffer.Add(CurrentDrawCmd);
        }

        public static void Draw()
        {
            if (GameManager.instance.IsAlive && GameManager.instance.Target.GetType().BaseType == SignatureManager.GClass49.Type.BaseType)
            {
                ESP.DrawPlayerEsp();
                ESP.DrawProjectiles();

                object gm = GameManager.instance.Target;

                object localPlayer = GameManager.GetLocalPLayer(gm);
                if(localPlayer != null && Player.GetHealth(localPlayer) > 0)
                {
                    Vec3 shhot_vec = CollisionHelper.GetShootVectorBase(localPlayer);
                    Vec3 head = Player.GetHeadPos(localPlayer);

                    if (Config.settings.esp_grenade_launcher && (Player.GetCurrentWeaponType(localPlayer) == ToolType.GLauncher || Player.GetCurrentWeaponType(localPlayer) == ToolType.C4))
                        ESP.DrawGrenade(gm, head, shhot_vec, Enum.ToObject(SignatureManager.WeaponType, (byte)ToolType.GLauncher), int.MaxValue, Color.green, 5, 50);

                    if (Config.settings.spread_croshair)
                    {
                        float r = (float)(CollisionHelper.CalcSpread(localPlayer) / GameManager.fov * Hooks.GL.W);
                        DrawCircleFilled(Hooks.GL.W / 2, Hooks.GL.H / 2, r, 50, new Color(50, 50, 50, 100), Color.transparent);
                    }

                }

            }
            Menu.Draw();

            BoneManager.InvalidateBones();
        }


    }
}
