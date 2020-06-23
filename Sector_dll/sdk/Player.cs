using Sector_dll.cheat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            if (SignatureManager.PlayerBase_crouching == null)
                return false;
            return ((bool[])SignatureManager.PlayerBase_crouching.GetValue(player))[0];
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

        public const double HeightStanding = 2.7;

        public const double HeightCrouching = 1.6;

    }
}
