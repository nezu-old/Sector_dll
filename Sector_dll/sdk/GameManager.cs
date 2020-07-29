using Sector_dll.cheat;
using Sector_dll.cheat.Hooks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sector_dll.sdk
{
    class GameManager
    {

        private static int SmallerScopeIndex = -1;

        public static object viewMatrix;

        public static Vec2 W2SResolution = new Vec2(1, 1);

        public static Vec2 ScreenResolution = new Vec2(1, 1);

        public static Vec2 W2SOffset = new Vec2(0, 0);

        public static WeakReference instance = new WeakReference(null);

        public static object GetViewMatrix(object self, object player)
        {
            object matrix2 = SignatureManager.GClass49_Base_matrix1.GetValue(self);
            if (player != null && IsScoped(self))
            {
                object weaponType = Player.GetCurrentWeaponType(player);
                byte scopeType = (byte)Helper.GetEquippedScopeType(player, weaponType);
                Vec2 ss = GetScopeSize(self, scopeType, weaponType);
                double ratio = ss.x / ss.y;
                double zoom = scopeType == 3 ? 4.7 : 5.1;
                double fov = 3.1415926535897931 * (Settings.GetFov(GetSettings(self)) / zoom / 180.0);
                object matrix3 = Matrix4.Generate(fov, ratio, 0.05, 4500.0);
                return Matrix4.Multiply(matrix2, matrix3);
            }
            object matrix1 = SignatureManager.GClass49_Base_matrix2.GetValue(self);
            return Matrix4.Multiply(matrix2, matrix1);
        }

        public static void NewFrame(object self)
        {
            object player = GetCurrentPLayer(self);
            viewMatrix = GetViewMatrix(self, player);
            ScreenResolution = new Vec2((double)SignatureManager.GClass49_Base_Base_ScreenWidth.GetValue(self),
                (double)SignatureManager.GClass49_Base_Base_ScreenHeight.GetValue(self));
            W2SResolution = ScreenResolution;

            if (player != null && IsScoped(self))
            {
                object weaponType = Player.GetCurrentWeaponType(player);
                byte scopeType = (byte)Helper.GetEquippedScopeType(player, weaponType);
                Vec2 ss = GetScopeSize(self, scopeType, weaponType);

                W2SOffset = new Vec2((W2SResolution.x - ss.x) / 2, (W2SResolution.y - ss.y) / 2);
                W2SResolution = ss;
            } 
            else
            {
                W2SOffset = new Vec2(0, 0);
            }
        }

        // 3: 4.7 - 4: 5.1

        public static bool W2s(object vec3, out object vec2)
        {
            if(viewMatrix == null)
            {
                vec2 = Vec2.New(-10000, -10000);
                return false;
            }
            Vec4 res = new Vec4(Vec4.Multiply(Vec4.New(vec3), viewMatrix));
            if (res.z < 0.0)
            {
                vec2 = Vec2.New(-10000, -10000);
                return false;
            }
            res.x /= res.w;
            res.y /= res.w;
            res.z /= res.w;

            double x = (res.x / 2.0 + 0.5) * W2SResolution.x;
            double y = (-res.y / 2.0 + 0.5) * W2SResolution.y;
            vec2 = Vec2.New(x + W2SOffset.x, y + W2SOffset.y);
            return true;
        }

        public static bool W2s(Vec3 vec3, out Vec2 vec2)
        {
            bool res = W2s(vec3.ToInternal(), out object r);
            vec2 = new Vec2(r);
            return res;
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

        public static Color GetPlayerColor(object self, object player)
        {
            return new Color(SignatureManager.GClass49_Base_GetPlayerColor.Invoke(self, new object[] { player }));
        }

        public static object GetMap(object self)
        {
            return SignatureManager.GCLass49_Base_Map.GetValue(self);
        }

        public static bool IsScoped(object self)
        {
            return (bool)SignatureManager.GClass49_Base_IsScoped.Invoke(self, new object[] { });
        }

        public static Vec2 GetScopeSize(object self, byte scopeType, object weaponType)
        {
            IDictionary s0 = SignatureManager.GClass49_Base_ScopeSizes1.GetValue(self) as IDictionary;
            IDictionary s1 = SignatureManager.GClass49_Base_ScopeSizes2.GetValue(self) as IDictionary;
            if(SmallerScopeIndex < 0)
                SmallerScopeIndex = s0.Count > s1.Count ? 0 : 1;
            if (scopeType == 3)
            {
                IDictionary sizes = SmallerScopeIndex == 0 ? s0 : s1;
                return new Vec2(sizes[weaponType]);
            }
            if (scopeType == 4)
            {
                IDictionary sizes = SmallerScopeIndex == 1 ? s0 : s1;
                return new Vec2(sizes[weaponType]);
            }
            throw new InvalidOperationException($"unsupported scope type {scopeType}");
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
