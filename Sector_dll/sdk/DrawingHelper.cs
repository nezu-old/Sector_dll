using Sector_dll.cheat;
using System;

namespace Sector_dll.sdk
{
    class DrawingHelper
    {

		[Flags]
		public enum Gravity
		{
			None = 0,
			Left = 1,
			Right = 2,
			CenterHorizontal = 4,
			Top = 8,
			Bottom = 16,
			CenterVertical = 32,
			Fill = 4096,
			AspectFit = 8192,
			AspectFill = 12288,
			TopLeft = 9,
			TopRight = 10,
			TopCenter = 12,
			BottomLeft = 17,
			BottomRight = 18,
			BottomCenter = 20,
			LeftCenter = 33,
			RightCenter = 34,
			Center = 36,
			HorizontalMask = 7,
			VerticalMask = 56,
			ScaleMask = 61440
		}

		public static void DrawRect(object d, object rect, double t, object color)
        {
            if (SignatureManager.DrawingHelper_DrawRect == null) 
                return;
            SignatureManager.DrawingHelper_DrawRect.Invoke(null, new object[] { d, rect, t, color });
        }

        public static void DrawFilledRect(object d, object rect, object color)
        {
            if (SignatureManager.Drawing_DrawFilledRect == null)
                return;
            SignatureManager.Drawing_DrawFilledRect.Invoke(d, new object[] { rect, color });
        }

        public static void DrawString(object d, string s, object rect, object font, object color, Gravity g)
        {
            if (SignatureManager.Drawing_DrawString == null)
                return;
            SignatureManager.Drawing_DrawString.Invoke(d, new object[] { s, rect, font, color, g, 0, 0 });
        }



    }
}
