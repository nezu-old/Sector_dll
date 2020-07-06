using Sector_dll.cheat;

namespace Sector_dll.sdk
{
    class Player
    {

        public static Vec3 GetOrigin(object player)
        {
            if (SignatureManager.PlayerBase_origin == null)
                return null;
            return new Vec3(SignatureManager.PlayerBase_origin.GetValue(player));
        }

        public static Vec3 GetHeadPos(object player)
        {
            if (SignatureManager.PlayerBase_origin == null)
                return null;
            Vec3 v = new Vec3(SignatureManager.PlayerBase_origin.GetValue(player));
            v.y += IsCrouching(player) ? HeightCrouching : HeightStanding;
            return v;
        }

        public static double GetHealth(object player)
        {
            if (SignatureManager.PlayerBase_health == null)
                return 0;
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
            if(SignatureManager.PLayerBase_EitherMod == null)
                return false;
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
            if (SignatureManager.PlayerBase_name == null)
                return "";
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
            if (SignatureManager.PLayerBase_CurrentWeaponIndex == null)
                return -1;
            return (int)SignatureManager.PLayerBase_CurrentWeaponIndex.Invoke(player, new object[] { });
        }

        public static object GetCurrentWeaponType(object player)
        {
            if (SignatureManager.PLayerBase_CurrentWeaponType == null)
                return null;
            return SignatureManager.PLayerBase_CurrentWeaponType.Invoke(player, new object[] { });
        }

        public const double HeightStanding = 2.7;

        public const double HeightCrouching = 1.6;

    }
}
