using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sectorsedge.cheat.Drawing
{
    public struct FontGlyph
    {
        

        public uint Codepoint;
        public bool Visible;
        public float AdvanceX;
        public float X0, Y0, X1, Y1;
        public float U0, V0, U1, V1;

        public FontGlyph(uint codepoint, bool visible, float advanceX, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1)
        {
            Codepoint = codepoint;
            Visible = visible;
            AdvanceX = advanceX;
            X0 = x0;
            Y0 = y0;
            X1 = x1;
            Y1 = y1;
            U0 = u0;
            V0 = v0;
            U1 = u1;
            V1 = v1;
        }
    }
}
