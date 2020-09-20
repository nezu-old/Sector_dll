using System.Runtime.InteropServices;

namespace sectorsedge.cheat.Drawing
{

    [StructLayout(LayoutKind.Explicit)]
    public struct DrawVert
    {
        public DrawVert(float x, float y, uint color)
        {
            posX = x;
            posY = y;
            uvX = 0;
            uvY = 0;
            col = color;
        }

        public DrawVert(float x, float y, float uvX, float uvY, uint color)
        {
            posX = x;
            posY = y;
            this.uvX = uvX;
            this.uvY = uvY;
            col = color;
        }

        [FieldOffset(0)]
        public float posX;
        [FieldOffset(4)]
        public float posY;
        [FieldOffset(8)]
        public float uvX;
        [FieldOffset(12)]
        public float uvY;
        [FieldOffset(16)]
        public uint col;
    }

}
