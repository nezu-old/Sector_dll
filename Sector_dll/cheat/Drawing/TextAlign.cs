using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sectorsedge.cheat.Drawing
{
    public enum TextAlign
    {
        DEFAULT  = 0,
        LEFT     = 1,
        RIGHT    = 2,
        TOP      = 4,
        BOTTOM   = 8,
        H_CENTER = LEFT | RIGHT,
        V_CENTER = TOP  | BOTTOM,
        CENTER   = H_CENTER | V_CENTER
    }
}
