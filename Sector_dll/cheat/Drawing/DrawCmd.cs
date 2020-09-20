namespace sectorsedge.cheat.Drawing
{
    class DrawCmd
    {
        public uint TextureId;
        public int VtxOffset;
        public int IdxOffset;
        public int ElemCount;

        public DrawCmd(uint textureId, int vtxOffset = 0, int idxOffset = 0)
        {
            TextureId = textureId;
            VtxOffset = vtxOffset;
            IdxOffset = idxOffset;
            ElemCount = 0;
        }

        public void IncrementElemCount(int by) => ElemCount += by;

    }
}
