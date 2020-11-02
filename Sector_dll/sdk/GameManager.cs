using Sector_dll.cheat;
using Sector_dll.cheat.Hooks;
using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sector_dll.sdk
{
    public enum PlayerFlags
    {
        None = 0,
        Visible = 1,
    }

    class GameManager
    {

        public static object viewMatrix;

        public static double fov = -1;

        public static Vec2 ScreenResolution = new Vec2(1, 1);

        public static WeakReference instance = new WeakReference(null);

        public static readonly int MaxPlayers = 32;

        public static PlayerFlags[] PlayerFlags = new PlayerFlags[MaxPlayers];

        private static FieldInfo matrix_22 = null;
        public static void NewFrame(object self)
        {
            viewMatrix = SignatureManager.GameManager_GetViewMatrix.Invoke(self, null);
            ScreenResolution = new Vec2(GL.W, GL.H);
            if (matrix_22 == null)
                matrix_22 = SignatureManager.Matrix4.GetFields(BindingFlags.Instance | BindingFlags.Public)[5];
            fov = 2.0 * Math.Atan(1.0 / (double)matrix_22.GetValue(viewMatrix));
        }

        public static bool W2s(Vec3 vec3, out Vec2 vec2)
        {
            if (viewMatrix == null)
            {
                vec2 = new Vec2(-10000, -10000);
                return false;
            }
            Vec4 res = new Vec4(Vec4.Multiply(Vec4.New(vec3), viewMatrix));
            if (res.z < 0.0)
            {
                vec2 = new Vec2(-10000, -10000);
                return false;
            }
            res.x /= res.w;
            res.y /= res.w;
            res.z /= res.w;

            double x = (res.x / 2.0 + 0.5) * ScreenResolution.x;
            double y = (-res.y / 2.0 + 0.5) * ScreenResolution.y;
            vec2 = new Vec2(x, y);
            return true;
        }

        public static List<object> GetPlayerList(object self)
        {
            return (SignatureManager.GClass49_player_list.GetValue(self) as IEnumerable<object>).Cast<object>().ToList();
        }

        public static object GetLocalPLayer(object self)
        {
            return SignatureManager.GClass49_Base_LocalPlayer.GetValue(self);
        }

        public static object GetCurrentPLayer(object self)
        {
            return SignatureManager.GClass49_Base_GetCurrentPLayer.Invoke(self, new object[] { });
        }

        public static Color GetPlayerColor(object self, object player, bool use_rel_colors)
        {
            return new Color(SignatureManager.GClass49_Base_GetPlayerColor.Invoke(self, new object[] { player, use_rel_colors }));
        }

        public static object GetMap(object self)
        {
            return SignatureManager.GCLass49_Base_Map.GetValue(self);
        }

        public static object GetMap() => GetMap(instance.Target);

        public static bool IsScoped(object self)
        {
            return (bool)SignatureManager.GClass49_Base_IsScoped.Invoke(self, new object[] { });
        }

        public static object GetSettings(object self)
        {
            return SignatureManager.GClass49_Base_Base_Settings.GetValue(self);
        }

        public static uint GenerateGlBuffersForPlayer(object self)
        {
            return (uint)SignatureManager.GClass49_Base_GenerateGlBuffersForPlayer.Invoke(self, new object[] { });
        }

        public static double OtherPlayerYOffset(object player)
        {
            return (double)SignatureManager.GameManager_OtherPlayerYOffset.Invoke(null, new object[] { player });
        }

        public static void SetupBones(object self, object player)
        {
            SignatureManager.GameManager_SetupBones.Invoke(self, new object[] { player });
        }

        public static List<object> GetPlayers(object self)
        {
            return (SignatureManager.GClass49_player_list.GetValue(self) as IEnumerable<object>).Cast<object>().ToList();
        }

        public static List<object> CollisionEntitys(object self)
        {
            return (SignatureManager.GameManager_CollisionEntityList.GetValue(self) as IEnumerable<object>).Cast<object>().ToList();
        }

        public static object GetPlayerByID(object gm, byte id)
        {
            return SignatureManager.GameManager_GetPlayerByID.Invoke(gm, new object[] { id });
        }

    }
}
