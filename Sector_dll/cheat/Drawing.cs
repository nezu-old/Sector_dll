using Sector_dll.sdk;
using Sector_dll.util;
using sectorsedge.cheat.Drawing;
using sectorsedge.cheat.Drawing.Fonts;
using sectorsedge.sdk;
using System;
using System.Collections.Generic;

namespace Sector_dll.cheat
{
    class Drawing
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

        public static void DrawString(string s, int x, int y, Color color)
        {
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

        public static void DrawRectFilled(int x, int y, int w, int h, Color color)
        {
            int idx = VtxBuffer.Count;
            float x2 = x + w;
            float y2 = y + h;
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

        public static void DrawRect(int x, int y, int w, int h, int t, Color color)
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

        public static void DrawLine(float x1, float y1, float x2, float y2, float t, Color color)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;

            float d2 = dx * dx + dy * dy; 
            if (d2 > 0.0f) { 
                float inv_len = 1.0f / (float)Math.Sqrt(d2); 
                dx *= inv_len; 
                dy *= inv_len; 
            }
            dx *= (t * 0.5f);
            dy *= (t * 0.5f);

            int idx = VtxBuffer.Count;

            VtxBuffer.Add(new DrawVert(x1 + dy, y1 - dx, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
            VtxBuffer.Add(new DrawVert(x2 + dy, y2 - dx, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
            VtxBuffer.Add(new DrawVert(x2 - dy, y2 + dx, TexUvWhitePixel.x, TexUvWhitePixel.y, color));
            VtxBuffer.Add(new DrawVert(x1 - dy, y1 + dx, TexUvWhitePixel.x, TexUvWhitePixel.y, color));

            IdxBuffer.Add(idx + 0);
            IdxBuffer.Add(idx + 1);
            IdxBuffer.Add(idx + 2);

            IdxBuffer.Add(idx + 0);
            IdxBuffer.Add(idx + 2);
            IdxBuffer.Add(idx + 3);

            CurrentDrawCmd.IncrementElemCount(6);
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
            DrawRectFilled(300, 100, 50, 50, Color.red);
            DrawRect(301, 101, 48, 48, 3, Color.green);
            DrawLine(304, 104, 346, 146, 1, Color.white);
            DrawLine(346, 104, 304, 146, 1, Color.white);
            //DrawString("The quick brown fox jumps over the lazy dog", 200, 200, Color.white);

            if (GameManager.instance.IsAlive && GameManager.instance.Target.GetType().BaseType == SignatureManager.GClass49.Type.BaseType)
            {
                ESP.DrawPlayerEsp();
            }
        }


    }
}
