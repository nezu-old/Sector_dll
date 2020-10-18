using Sector_dll.cheat;
using Sector_dll.util;
using System;
using System.Linq;

namespace Sector_dll.sdk
{
    class Player
    {

        public static object New(object gm, byte id)
        {
            object glThingy = GameManager.GenerateGlBuffersForPlayer(gm);
            return SignatureManager.Player_BotConstructor.Invoke(new object[] { gm, id, glThingy });
        }

        public static Vec3 GetOrigin(object player)
        {
            return new Vec3(SignatureManager.PlayerBase_origin.GetValue(player));
        }

        public static void SetOrigin(object player, Vec3 pos)
        {
            SignatureManager.PlayerBase_origin.SetValue(player, pos.ToInternal());
        }

        public static double GetCrouchProgress(object player)
        {
            object watch = SignatureManager.PlayerBase_Base_CrouchWatch.GetValue(player);
            return CustomWatch.GetProgress(watch);
        }

        public static Vec3 GetHeadPos(object player)
        {
            Vec3 v = new Vec3(SignatureManager.PlayerBase_origin.GetValue(player));
            v.y += Helper.Lerp(HeightStanding, HeightCrouching, GetCrouchProgress(player));
            return v;
        }

        public static double GetHealth(object player)
        {
            return (double)SignatureManager.PlayerBase_health.GetValue(player);
        }

        public static bool EitherMod(object p, ModType mod)
        {
            return (bool)SignatureManager.PLayerBase_EitherMod.Invoke(p, new object[] { mod });
        }

        public static double GetMaxHealth(object player)
        {
            //TODO: infected team type == 300 // nevrmind that sit dead XD
            int num = 100;
            if (EitherMod(player, ModType.Shielding))
            {
                num += 10;
            }
            if (EitherMod(player, ModType.Speed))
            {
                num += -10;
            }
            return (double)num;
        }

        public static string GetName(object player)
        {
            return (string)SignatureManager.PlayerBase_name.GetValue(player);
        }

        public static int GetCurrentWeaponIndex(object player)
        {
            return (int)SignatureManager.PLayerBase_CurrentWeaponIndex.Invoke(player, new object[] { });
        }

        public static ToolType GetCurrentWeaponType(object player)
        {
            return (ToolType)(byte)SignatureManager.PLayerBase_CurrentWeaponType.Invoke(player, new object[] { });
        }

        public static object GetCurrentWeapon(object player)
        {
            return SignatureManager.PlayerBase_Base_GetCurrentWeapon.Invoke(player, null);
        }

        public static TeamType GetTeam(object player)
        {
            object team = SignatureManager.PlayerBase_Base_GetTeam.Invoke(player, new object[] { });
            return (TeamType)(byte)Convert.ChangeType(team, Enum.GetUnderlyingType(team.GetType()));
        }

        public static void SetTeam(object player, TeamType team)
        {
            object team_obj = Enum.ToObject(SignatureManager.TeamType, (byte)team);
            SignatureManager.PlayerBase_Base_SetTeam.Invoke(player, new[] { team_obj });
        }

        public static byte GetSkinId(object player)
        {
            object characterTexture = SignatureManager.PlayerBase_Base_CharacterTexture.GetValue(player);
            object playerType = SignatureManager.CharacterTexture_PlayerType.GetValue(characterTexture);
            return (byte)Convert.ChangeType(playerType, Enum.GetUnderlyingType(playerType.GetType()));
        }

        public static object GetBones(object player)
        {
            return SignatureManager.Player_GetBones.Invoke(player, new object[] { });
        }

        public static object[] GetBoneTransforms(object player)
        {
            return (SignatureManager.Player_BoneTransforms.GetValue(player) as Array).Cast<object>().ToArray();
        }

        public static double GetPitch(object player)
        {
            return (double)SignatureManager.PlayerBase_Base_Pitch.GetValue(player);
        }

        public static double GetYaw(object player)
        {

            return (double)SignatureManager.PlayerBase_Base_Yaw.GetValue(player);
        }

        public static void SetPitch(object player, double pitch)
        {
            SignatureManager.PlayerBase_Base_Pitch.SetValue(player, Math.Min(1.5607963267948965, Math.Max(-1.5607963267948965, pitch)));
        }

        public static void SetYaw(object player, double yaw)
        {
            while (yaw > 3.141591653589793) yaw += -3.141591653589793 * 2;
            while (yaw < -3.141591653589793) yaw += 3.141591653589793 * 2;
            SignatureManager.PlayerBase_Base_Yaw.SetValue(player, yaw);
        }

        public static Vec3 GetLookAtVector(object player)
        {
            double pitch = GetPitch(player);
            double yaw = GetYaw(player);
            double cos_pitch = Math.Cos(pitch);
            return new Vec3(cos_pitch * Math.Sin(yaw), Math.Sin(pitch), cos_pitch * Math.Cos(yaw));
        }

        public static bool IsScopped(object player)
        {
            return (bool)SignatureManager.PlayerBase_Base_IsScoped.Invoke(player, null);
        }

        public const double HeightStanding = 2.52;

        public const double HeightCrouching = 1.6;

    }
}
