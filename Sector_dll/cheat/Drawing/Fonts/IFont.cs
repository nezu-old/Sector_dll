using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sectorsedge.cheat.Drawing.Fonts
{
    interface IFont
    {

        FontGlyph FindFontGlyph(char c);

        byte[] GetTextureData(out int w, out int h);

        uint Texture_id { get; set; }

        int Size { get; }

    }
}
