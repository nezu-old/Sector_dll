using Sector_dll.cheat;
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

        public static Vec3 GetHeadPos(object player)
        {
            Vec3 v = new Vec3(SignatureManager.PlayerBase_origin.GetValue(player));
            v.y += IsCrouching(player) ? HeightCrouching : HeightStanding;
            return v;
        }

        public static double GetHealth(object player)
        {
            return (double)SignatureManager.PlayerBase_health.GetValue(player);
        }

        public static bool IsCrouching(object player)
        {
            //if (SignatureManager.PlayerBase_crouching == null)
            //    return false;
            //return ((bool[])SignatureManager.PlayerBase_crouching.GetValue(player))[0];
            return false;
        }

        public static bool EitherMod(object p, ModType mod)
        {
            return (bool)SignatureManager.PLayerBase_EitherMod.Invoke(p, new object[] { mod });
        }

        public static double GetMaxHealth(object player)
        {
            //TODO: infected team type == 300
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

        //public static object GenerateHistoryPlayer(object player)
        //{
        //    if (SignatureManager.GenerateHistoryPlayer == null)
        //        return null;
        //    return SignatureManager.GenerateHistoryPlayer.Invoke(null, new object[] { player });
        //}

        public static int GetCurrentWeaponIndex(object player)
        {
            return (int)SignatureManager.PLayerBase_CurrentWeaponIndex.Invoke(player, new object[] { });
        }

        public static object GetCurrentWeaponType(object player)
        {
            return SignatureManager.PLayerBase_CurrentWeaponType.Invoke(player, new object[] { });
        }

        public static void SetTeam(object player, TeamType team)
        {
            object team_obj = Enum.ToObject(SignatureManager.TeamType, (byte)team);
            SignatureManager.PLayerBase_Base_SetTeam.Invoke(player, new[] { team_obj });
        }

        public static object GetBones(object player)
        {
            return SignatureManager.Player_GetBones.Invoke(player, new object[] { });
        }

        public static object[] GetBoneTransforms(object player)
        {
            return (SignatureManager.Player_BoneTransforms.GetValue(player) as Array).Cast<object>().ToArray();
        }

        public const double HeightStanding = 2.7;

        public const double HeightCrouching = 1.6;

    }
}
