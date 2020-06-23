using Sector_dll.cheat;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
    class GameManager
    {

        public static object GetViewMatrix(object self)
        {
            object matrix1 = SignatureManager.GClass49_Base_matrix1.GetValue(self);
            object matrix2 = SignatureManager.GClass49_Base_matrix2.GetValue(self);
            return Matrix4.Multiply(matrix2, matrix1);
        }

        public static bool W2s(object self, object vec3, out object vec2)
        {
            Vec4 res = new Vec4(Vec4.Multiply(Vec4.New(vec3), GetViewMatrix(self)));
            if (res.z < 0.0)
            {
                vec2 = Vec2.New(-10000, -10000);
                return false;
            }
            res.x /= res.w;
            res.y /= res.w;
            res.z /= res.w;

            double x = (res.x / 2.0 + 0.5) * (double)SignatureManager.GClass49_Base_Base_ScreenWidth.GetValue(self);
            double y = (-res.y / 2.0 + 0.5) * (double)SignatureManager.GClass49_Base_Base_ScreenHeight.GetValue(self);
            vec2 = Vec2.New(x, y);
            return true;
        }

        public static bool W2s(object self, Vec3 vec3, out Vec2 vec2)
        {
            bool res = W2s(self, vec3.ToInternal(), out object r);
            vec2 = new Vec2(r);
            return res;
        }

        public static List<object> GetPlayerList(object self)
        {
            return (SignatureManager.GClass49_player_list.GetValue(self) as IEnumerable<object>).Cast<object>().ToList();
        }

        public static object GetLocalPLayer(object self)
        {
            if (SignatureManager.GClass49_Base_LocalPlayer == null)
                return null;
            return SignatureManager.GClass49_Base_LocalPlayer.GetValue(self);
        }

        public static object GetPlayerColor(object self, object player)
        {
            if (SignatureManager.GClass49_Base_GetPlayerColor == null)
                return null;
            return SignatureManager.GClass49_Base_GetPlayerColor.Invoke(self, new object[] { player });
        }

    }
}
